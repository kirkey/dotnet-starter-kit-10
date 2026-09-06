using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Jobs.Services;
using FSH.Framework.Shared.Multitenancy;
using FSH.Framework.Shared.Storage;
using FSH.Framework.Storage;
using FSH.Framework.Storage.Services;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Domain;
using FSH.Modules.Ai.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Modules.Ai.Jobs;

/// <summary>
/// Fetches a web source's page, converts it to readable Markdown, and stores the twin.
/// Failures are terminal and visible (source → Failed with reason); refresh re-runs.
/// Chunking is chained once conversion succeeds.
/// </summary>
public sealed class WebSourceFetchJob(
    IWebPageFetcher fetcher,
    IHtmlToMarkdownConverter converter,
    IStorageService storage,
    IJobService jobs,
    IServiceScopeFactory scopeFactory,
    ILogger<WebSourceFetchJob> logger)
{
    public async Task FetchAsync(string tenantId, Guid sourceId, CancellationToken ct)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantId);

        // Fresh scope, tenant first, then the DbContext — so its Finbuckle filter reads a real
        // TenantInfo instead of the null one the job's outer scope carries.
        using var scope = scopeFactory.CreateScope();
        var tenant = await scope.ServiceProvider
            .GetRequiredService<IMultiTenantStore<AppTenantInfo>>()
            .GetAsync(tenantId)
            .ConfigureAwait(false);
        if (tenant is null)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                logger.LogWarning("[Ai] web-fetch skipped: tenant '{TenantId}' not found", tenantId);
            }

            return;
        }

        scope.ServiceProvider.GetRequiredService<IMultiTenantContextSetter>().MultiTenantContext =
            new MultiTenantContext<AppTenantInfo>(tenant);

        var db = scope.ServiceProvider.GetRequiredService<AiDbContext>();

        var source = await db.Sources
            .FirstOrDefaultAsync(s => s.Id == sourceId, ct)
            .ConfigureAwait(false);
        if (source is null)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                logger.LogWarning("[Ai] web-fetch skipped: source {SourceId} not found", sourceId);
            }

            return;
        }

        if (source.Kind != AiSourceKind.WebLink)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                logger.LogWarning("[Ai] web-fetch skipped: source {SourceId} is not a web link", sourceId);
            }

            return;
        }

        try
        {
            var converted = false;
            var page = await fetcher.FetchAsync(source.SourceRef, ct).ConfigureAwait(false);
            var (title, markdown) = converter.Convert(page.Html, source.Name);
            if (string.IsNullOrWhiteSpace(markdown))
            {
                source.MarkFailed($"'{source.SourceRef}' yielded no readable content.");
            }
            else
            {
                if (source.MdStorageKey is not null)
                {
                    try
                    {
                        await storage.RemoveAsync(source.MdStorageKey, ct).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(ex, "[Ai] could not remove stale twin {Key}", source.MdStorageKey);
                    }
                }

                var twinKey = await storage.UploadAsync<AiSource>(
                    new FileUploadRequest
                    {
                        FileName = $"{source.Id:N}.md",
                        ContentType = "text/markdown",
                        Data = [.. System.Text.Encoding.UTF8.GetBytes(markdown)]
                    },
                    FileType.Document,
                    ct).ConfigureAwait(false);

                source.AttachMarkdownTwin(twinKey);
                source.MarkReady();
                converted = true;
            }

            await db.SaveChangesAsync(ct).ConfigureAwait(false);

            if (converted)
            {
                jobs.Enqueue<ChunkAndEmbedJob>(j => j.ProcessAsync(tenantId, source.Id, CancellationToken.None));
            }

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(
                    "[Ai] web source {SourceId} ({Title}) fetched: {Status}",
                    source.Id, title, source.Status);
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            source.MarkFailed(TrimReason(ex.Message));
            await db.SaveChangesAsync(ct).ConfigureAwait(false);

            if (logger.IsEnabled(LogLevel.Warning))
            {
                logger.LogWarning(ex, "[Ai] web source {SourceId} fetch failed", sourceId);
            }
        }
    }

    private static string TrimReason(string message) =>
        message.Length <= 500 ? message : message[..500];
}
