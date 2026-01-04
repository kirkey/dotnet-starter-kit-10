// Module.Auditing/AuditHttpMiddleware.cs

using FSH.Module.Auditing.Contracts;
using FSH.Module.Auditing.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace FSH.Module.Auditing.Infrastructure.Http;

public sealed class AuditHttpMiddleware(RequestDelegate next, AuditHttpOptions opts, IAuditPublisher publisher)
{
    public async Task InvokeAsync(HttpContext ctx)
    {
        ArgumentNullException.ThrowIfNull(ctx);

        if (ShouldSkip(ctx))
        {
            await next(ctx);
            return;
        }

        IAuditMaskingService? masker = ctx.RequestServices.GetService<IAuditMaskingService>();
        Stopwatch sw = Stopwatch.StartNew();

        object? reqPreview = null;
        int reqSize = 0;
        if (opts.CaptureBodies &&
            ContentTypeHelper.IsJsonLike(ctx.Request.ContentType, opts.AllowedContentTypes))
        {
            (reqPreview, reqSize) = await HttpBodyReader.ReadRequestAsync(ctx, opts.MaxRequestBytes, ctx.RequestAborted);
            if (reqPreview is not null && masker is not null)
            {
                reqPreview = masker.ApplyMasking(reqPreview);
            }
        }

        Stream originalBody = ctx.Response.Body;
        await using MemoryStream tee = new();
        await using MemoryStream respBuffer = new();
        ctx.Response.Body = tee;

        try
        {
            await next(ctx);
            sw.Stop();

            object? respPreview = null;
            int respSize = 0;

            if (opts.CaptureBodies &&
                ContentTypeHelper.IsJsonLike(ctx.Response.ContentType, opts.AllowedContentTypes))
            {
                tee.Position = 0;
                await tee.CopyToAsync(respBuffer, ctx.RequestAborted);
                (respPreview, respSize) = await HttpBodyReader.ReadResponseAsync(
                    respBuffer, opts.MaxResponseBytes, ctx.RequestAborted);
                if (respPreview is not null && masker is not null)
                {
                    respPreview = masker.ApplyMasking(respPreview);
                }
            }

            respBuffer.Position = 0;
            ctx.Response.Body = originalBody;
            if (respBuffer.Length > 0)
                await respBuffer.CopyToAsync(originalBody, ctx.RequestAborted);

            await Audit.ForActivity(Contracts.ActivityKind.Http, ctx.Request.Path)
                .WithActivityResult(
                    statusCode: ctx.Response.StatusCode,
                    durationMs: (int)sw.Elapsed.TotalMilliseconds,
                    captured: opts.CaptureBodies ? BodyCapture.Both : BodyCapture.None,
                    requestSize: reqSize,
                    responseSize: respSize,
                    requestPreview: reqPreview,
                    responsePreview: respPreview)
                .WithSource("api")
                .WithTenant((publisher.CurrentScope?.TenantId) ?? null)
                .WithUser(publisher.CurrentScope?.UserId, publisher.CurrentScope?.UserName)
                .WithCorrelation(publisher.CurrentScope?.CorrelationId ?? ctx.TraceIdentifier)
                .WithRequestId(publisher.CurrentScope?.RequestId ?? ctx.TraceIdentifier)
                .WriteAsync(ctx.RequestAborted);
        }
        catch (Exception ex)
        {
            sw.Stop();

            AuditSeverity sev = ExceptionSeverityClassifier.Classify(ex);
            if (sev >= opts.MinExceptionSeverity)
            {
                await Audit.ForException(ex, ExceptionArea.Api,
                        routeOrLocation: ctx.Request.Path, severity: sev)
                    .WithSource("api")
                    .WithTenant((publisher.CurrentScope?.TenantId) ?? null)
                    .WithUser(publisher.CurrentScope?.UserId, publisher.CurrentScope?.UserName)
                    .WithCorrelation(publisher.CurrentScope?.CorrelationId ?? ctx.TraceIdentifier)
                    .WithRequestId(publisher.CurrentScope?.RequestId ?? ctx.TraceIdentifier)
                    .WriteAsync(ctx.RequestAborted);
            }

            ctx.Response.Body = originalBody;
            throw;
        }
    }

    private bool ShouldSkip(HttpContext ctx)
    {
        string path = ctx.Request.Path.Value ?? string.Empty;
        return opts.ExcludePathStartsWith.Any(prefix =>
            path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
    }
}
