using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using FSH.Framework.Caching;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FSH.Framework.Web.Idempotency;

/// <summary>
/// Endpoint filter that provides idempotency for POST/PUT/PATCH requests.
/// When an Idempotency-Key header is present, the response is cached and replayed
/// for subsequent requests with the same key.
/// </summary>
/// <remarks>
/// Reads AND writes go through <see cref="IDistributedCache"/> directly with explicit JSON.
/// A previous revision wrote via <c>HybridCache</c> while probing via <c>IDistributedCache</c>;
/// HybridCache namespaces its L2 keys (invisible to raw reads), so replays never hit.
/// Entries expire by TTL; nothing purges them by tag.
/// </remarks>
public sealed class IdempotencyEndpointFilter : IEndpointFilter
{
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>Upper bound for a cached response body; larger responses skip caching.</summary>
    private const int MaxCacheBytes = 1024 * 1024;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        var httpContext = context.HttpContext;
        var options = httpContext.RequestServices.GetRequiredService<IOptions<IdempotencyOptions>>().Value;
        var idempotencyKey = httpContext.Request.Headers[options.HeaderName].ToString();

        // No header = pass through (idempotency is opt-in per request)
        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            return await next(context).ConfigureAwait(false);
        }

        if (idempotencyKey.Length > options.MaxKeyLength)
        {
            return TypedResults.BadRequest($"Idempotency key exceeds maximum length of {options.MaxKeyLength}.");
        }

        var distributedCache = httpContext.RequestServices.GetRequiredService<IDistributedCache>();
        var logger = httpContext.RequestServices.GetRequiredService<ILogger<IdempotencyEndpointFilter>>();

        // Include tenant context in cache key for isolation
        var tenantId = httpContext.User.FindFirst("tenant")?.Value ?? "global";
        var cacheKey = CacheKeys.IdempotencyEntry(tenantId, idempotencyKey);

        // Probe read via IDistributedCache (real GetAsync, null on miss).
        var cachedBytes = await distributedCache.GetAsync(cacheKey, httpContext.RequestAborted).ConfigureAwait(false);
        if (cachedBytes is not null && cachedBytes.Length > 0)
        {
            var cached = JsonSerializer.Deserialize<CachedIdempotentResponse>(cachedBytes, JsonOpts);
            if (cached is not null)
            {
                if (logger.IsEnabled(LogLevel.Debug))
                {
                    logger.LogDebug("Idempotent replay for key {KeyHash}", HashKey(idempotencyKey));
                }
                httpContext.Response.Headers["Idempotency-Replayed"] = "true";
                httpContext.Response.StatusCode = cached.StatusCode;
                if (cached.ContentType is not null)
                {
                    httpContext.Response.ContentType = cached.ContentType;
                }

                if (cached.Body.Length > 0)
                {
                    await httpContext.Response.Body.WriteAsync(cached.Body, httpContext.RequestAborted).ConfigureAwait(false);
                }

                // Inert status result: preserves the replayed status without appending a body
                // (returning null would serialize a literal "null" onto the wire).
                return Results.StatusCode(cached.StatusCode);
            }
        }

        // Execute the handler
        var result = await next(context).ConfigureAwait(false);

        // Cache the response through the same IDistributedCache the probe reads, so the
        // bytes round-trip byte-for-byte.
        if (result is IStatusCodeHttpResult && result is IResult irisult && !httpContext.Response.HasStarted)
        {
            // IResult (Results.Ok/Created/...) only becomes bytes when executed: tee the single
            // execution into a buffer so the cached payload matches the wire exactly (caching the
            // result object itself would replay its JSON envelope instead of the real body).
            // Status-code results are pure serialization; files/streams are single-consumption
            // and skip caching entirely.
            return new TeeResult(irisult, body => CacheResponseAsync(
                distributedCache, cacheKey, body, httpContext, options, logger, httpContext.RequestAborted));
        }

        if (result is not null && result is not IResult)
        {
            try
            {
                var body = JsonSerializer.SerializeToUtf8Bytes(result, JsonOpts);
                if (body.Length <= MaxCacheBytes)
                {
                    await CacheResponseAsync(
                        distributedCache, cacheKey, body, httpContext, options, logger, httpContext.RequestAborted)
                        .ConfigureAwait(false);
                }
            }
            // Best-effort caching: idempotency replay is a convenience, not a correctness requirement
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogWarning(ex, "Failed to cache idempotent response for key {KeyHash}", HashKey(idempotencyKey));
            }
        }

        return result;
    }

    private static async Task CacheResponseAsync(
        IDistributedCache distributedCache,
        string cacheKey,
        byte[] body,
        HttpContext httpContext,
        IdempotencyOptions options,
        ILogger logger,
        CancellationToken ct)
    {
        try
        {
            var responseToCache = new CachedIdempotentResponse
            {
                StatusCode = httpContext.Response.StatusCode is > 0 and < 600 ? httpContext.Response.StatusCode : 200,
                ContentType = httpContext.Response.ContentType ?? "application/json",
                Body = body
            };

            var payload = JsonSerializer.SerializeToUtf8Bytes(responseToCache, JsonOpts);
            await distributedCache.SetAsync(
                cacheKey,
                payload,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = options.DefaultTtl },
                ct).ConfigureAwait(false);
        }
        // Best-effort caching: idempotency replay is a convenience, not a correctness requirement
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogWarning(ex, "Failed to cache idempotent response for key {KeyHash}", HashKey(cacheKey));
        }
    }

    /// <summary>
    /// Executes the inner result exactly once while teeing its bytes to a capture callback.
    /// </summary>
    private sealed class TeeResult(IResult inner, Func<byte[], Task> onCaptured) : IResult
    {
        public async Task ExecuteAsync(HttpContext httpContext)
        {
            ArgumentNullException.ThrowIfNull(httpContext);

            var originalBody = httpContext.Response.Body;
            await using var buffer = new MemoryStream();
            httpContext.Response.Body = buffer;
            try
            {
                await inner.ExecuteAsync(httpContext).ConfigureAwait(false);
            }
            finally
            {
                httpContext.Response.Body = originalBody;
            }

            var body = buffer.ToArray();
            await originalBody.WriteAsync(body, httpContext.RequestAborted).ConfigureAwait(false);
            if (body.Length <= MaxCacheBytes)
            {
                await onCaptured(body).ConfigureAwait(false);
            }
        }
    }

    private static string HashKey(string key)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        return Convert.ToHexString(hash.AsSpan(0, 8));
    }
}

public static class IdempotencyEndpointExtensions
{
    /// <summary>
    /// Enables idempotency for this endpoint. Requires Idempotency-Key header on requests.
    /// Duplicate requests with the same key return the cached response.
    /// </summary>
    public static RouteHandlerBuilder WithIdempotency(this RouteHandlerBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        return builder.AddEndpointFilter<IdempotencyEndpointFilter>();
    }
}
