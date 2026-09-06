using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Shared.Multitenancy;
using FSH.Framework.Storage.Services;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Modules.Ai.Jobs;

/// <summary>
/// Splits a converted source's Markdown twin into chunks and attaches provider embeddings.
/// Terminal failures mark the source Failed with a reason (refresh/rechunk recovers).
/// </summary>
public sealed class ChunkAndEmbedJob(
    IStorageService storage,
    IAiTextChunker chunker,
    IServiceScopeFactory scopeFactory,
    ILogger<ChunkAndEmbedJob> logger)
{
    public async Task ProcessAsync(string tenantId, Guid sourceId, CancellationToken ct)
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
                logger.LogWarning("[Ai] chunk skipped: tenant '{TenantId}' not found", tenantId);
            }

            return;
        }

        scope.ServiceProvider.GetRequiredService<IMultiTenantContextSetter>().MultiTenantContext =
            new MultiTenantContext<AppTenantInfo>(tenant);

        var db = scope.ServiceProvider.GetRequiredService<AiDbContext>();
        var selector = scope.ServiceProvider.GetRequiredService<IAiProviderSelector>();

        var source = await db.Sources
            .FirstOrDefaultAsync(s => s.Id == sourceId, ct)
            .ConfigureAwait(false);
        if (source is null)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                logger.LogWarning("[Ai] chunk skipped: source {SourceId} not found", sourceId);
            }

            return;
        }

        if (source.MdStorageKey is null
            || (source.Status != AiSourceStatus.Ready && source.Status != AiSourceStatus.Failed))
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(
                    "[Ai] chunk skipped: source {SourceId} is {Status} without a convertible twin",
                    sourceId, source.Status);
            }

            return;
        }

        try
        {
            var download = await storage.DownloadAsync(source.MdStorageKey, ct).ConfigureAwait(false);
            if (download is null)
            {
                source.MarkFailed($"Markdown twin '{source.MdStorageKey}' is missing from storage.");
                await db.SaveChangesAsync(ct).ConfigureAwait(false);
                return;
            }

            await using (download.Stream.ConfigureAwait(false))
            {
                using var reader = new StreamReader(download.Stream, System.Text.Encoding.UTF8);
                var markdown = await reader.ReadToEndAsync(ct).ConfigureAwait(false);
                if (string.IsNullOrWhiteSpace(markdown))
                {
                    if (logger.IsEnabled(LogLevel.Warning))
                    {
                        logger.LogWarning("[Ai] chunk skipped: source {SourceId} twin is empty", sourceId);
                    }

                    return;
                }

                var texts = chunker.Chunk(markdown);
                var embedder = await selector.SelectEmbeddingClientAsync(ct).ConfigureAwait(false);

                await db.Chunks
                    .Where(c => c.SourceId == sourceId)
                    .ExecuteDeleteAsync(ct)
                    .ConfigureAwait(false);

                foreach (var text in texts)
                {
                    var vector = await embedder.EmbedAsync(text, ct).ConfigureAwait(false);
                    var chunk = source.AddChunk(text);
                    chunk.AttachEmbedding(vector, embedder.ModelName);
                    // Explicit state (see ChatRunner): the source may be dirtied below (MarkReady
                    // on recovered failures), which would otherwise mis-track fresh rows as Modified.
                    db.Entry(chunk).State = EntityState.Added;
                }

                if (source.Status == AiSourceStatus.Failed)
                {
                    source.MarkReady();
                }

                await db.SaveChangesAsync(ct).ConfigureAwait(false);

                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation(
                        "[Ai] source {SourceId} chunked into {Count} chunks ({Model})",
                        sourceId, texts.Count, embedder.ModelName);
                }
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            source.MarkFailed(TrimReason(ex.Message));
            await db.SaveChangesAsync(ct).ConfigureAwait(false);

            if (logger.IsEnabled(LogLevel.Warning))
            {
                logger.LogWarning(ex, "[Ai] source {SourceId} chunk/embed failed", sourceId);
            }
        }
    }

    private static string TrimReason(string message) =>
        message.Length <= 500 ? message : message[..500];
}
