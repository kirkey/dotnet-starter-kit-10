using FSH.Framework.Eventing.Abstractions;
using FSH.Framework.Shared.Storage;
using FSH.Framework.Storage;
using FSH.Framework.Storage.Services;
using FSH.Framework.Jobs.Services;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Domain;
using FSH.Modules.Ai.Jobs;
using FSH.Modules.Files.Contracts.Events;
using FSH.Modules.Files.Contracts.v1.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;

namespace FSH.Modules.Ai.IntegrationEventHandlers;

/// <summary>
/// Ingests finalized file uploads into the knowledge base: downloads the original bytes,
/// normalizes supported types (markdown/text) to a Markdown twin stored beside the original,
/// and marks the source Ready. Unsupported types or missing bytes produce a Failed source with
/// a reason instead of failing silently. Idempotent: the Inbox dedups redeliveries and the
/// unique SourceRef index plus the pre-check below cover the rest.
/// </summary>
public sealed class FileFinalizedIngestionHandler(
    AiDbContext db,
    IStorageService storage,
    IJobService jobs,
    ILogger<FileFinalizedIngestionHandler> logger)
    : IIntegrationEventHandler<FileFinalizedIntegrationEvent>
{
    /// <summary>Upper bound for in-memory text extraction; larger files are marked Failed.</summary>
    private const long MaxExtractionBytes = 25 * 1024 * 1024;

    public async Task HandleAsync(FileFinalizedIntegrationEvent @event, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(@event);
        var tenantId = @event.TenantId;
        if (string.IsNullOrWhiteSpace(tenantId))
        {
            throw new InvalidOperationException("FileFinalizedIntegrationEvent is missing TenantId.");
        }

        // Only clean files enter the knowledge base; quarantined uploads are ignored.
        if (@event.FinalStatus != (int)FileAssetStatus.Available)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(
                    "[Ai] ignoring file {FileAssetId} with status {FinalStatus}; only Available files are ingested",
                    @event.FileAssetId, @event.FinalStatus);
            }

            return;
        }

        var sourceRef = @event.FileAssetId.ToString();
        var existing = await db.Sources
            .FirstOrDefaultAsync(x => x.SourceRef == sourceRef, ct)
            .ConfigureAwait(false);
        if (existing is not null)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(
                    "[Ai] file {FileAssetId} already ingested as source {SourceId} ({Status}); skipping",
                    @event.FileAssetId, existing.Id, existing.Status);
            }

            return;
        }

        var name = string.IsNullOrWhiteSpace(@event.OriginalFileName)
            ? $"{@event.FileAssetId:N}"
            : @event.OriginalFileName;

        if (string.IsNullOrWhiteSpace(@event.StorageKey))
        {
            await SaveFailedAsync(name, @event.FileAssetId,
                "File finalized before storage-key propagation; re-upload to ingest.", ct)
                .ConfigureAwait(false);
            return;
        }

        if (!IsSupported(@event.ContentType, name))
        {
            await SaveFailedAsync(name, @event.FileAssetId,
                $"Content type '{@event.ContentType}' is not ingestible; markdown and plain text are supported.",
                ct).ConfigureAwait(false);
            return;
        }

        if (@event.SizeBytes > MaxExtractionBytes)
        {
            await SaveFailedAsync(name, @event.FileAssetId,
                $"File size {@event.SizeBytes} bytes exceeds the {MaxExtractionBytes} byte extraction limit.",
                ct).ConfigureAwait(false);
            return;
        }

        var download = await storage.DownloadAsync(@event.StorageKey, ct).ConfigureAwait(false);
        if (download is null)
        {
            await SaveFailedAsync(name, @event.FileAssetId,
                $"Original bytes not found at storage key '{@event.StorageKey}'.", ct)
                .ConfigureAwait(false);
            return;
        }

        await using (download.Stream.ConfigureAwait(false))
        {
            using var buffer = new MemoryStream();
            await download.Stream.CopyToAsync(buffer, ct).ConfigureAwait(false);
            var markdown = DecodeText(buffer.ToArray());
            if (string.IsNullOrWhiteSpace(markdown))
            {
                await SaveFailedAsync(name, @event.FileAssetId,
                    "File contains no extractable text.", ct).ConfigureAwait(false);
                return;
            }

            // Plain text is already valid Markdown; .md payloads pass through unchanged.
            var twinKey = await storage.UploadAsync<AiSource>(
                new FileUploadRequest
                {
                    FileName = $"{@event.FileAssetId:N}.md",
                    ContentType = "text/markdown",
                    Data = [.. Encoding.UTF8.GetBytes(markdown)]
                },
                FileType.Document,
                ct).ConfigureAwait(false);

            var source = AiSource.CreateFileSource(name, @event.FileAssetId);
            source.AttachMarkdownTwin(twinKey);
            source.MarkReady();
            db.Sources.Add(source);
            await db.SaveChangesAsync(ct).ConfigureAwait(false);

            jobs.Enqueue<ChunkAndEmbedJob>(j => j.ProcessAsync(tenantId, source.Id, CancellationToken.None));

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(
                    "[Ai] ingested file {FileAssetId} as source {SourceId} ({Chars} chars, twin {TwinKey})",
                    @event.FileAssetId, source.Id, markdown.Length, twinKey);
            }
        }
    }

    private async Task SaveFailedAsync(string name, Guid fileAssetId, string reason, CancellationToken ct)
    {
        var source = AiSource.CreateFileSource(name, fileAssetId);
        source.MarkFailed(reason);
        db.Sources.Add(source);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);

        if (logger.IsEnabled(LogLevel.Warning))
        {
            logger.LogWarning("[Ai] file {FileAssetId} ingestion failed: {Reason}", fileAssetId, reason);
        }
    }

    private static bool IsSupported(string contentType, string fileName)
    {
        if (contentType.StartsWith("text/markdown", StringComparison.OrdinalIgnoreCase)
            || contentType.StartsWith("text/plain", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var extension = Path.GetExtension(fileName);
        return extension.Equals(".md", StringComparison.OrdinalIgnoreCase)
            || extension.Equals(".markdown", StringComparison.OrdinalIgnoreCase)
            || extension.Equals(".txt", StringComparison.OrdinalIgnoreCase);
    }

    private static string DecodeText(byte[] bytes)
    {
        // Strip UTF-8 BOM when present so the twin starts clean.
        const byte b1 = 0xEF, b2 = 0xBB, b3 = 0xBF;
        var text = bytes.Length >= 3 && bytes[0] == b1 && bytes[1] == b2 && bytes[2] == b3
            ? Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3)
            : Encoding.UTF8.GetString(bytes);
        return text.Trim();
    }
}
