using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.Dtos;

namespace FSH.Modules.Ai.Domain;

/// <summary>
/// Knowledge source aggregate — owns its chunk collection. A source is either an
/// uploaded file (with a Markdown twin in storage) or a fetched web link.
/// State transitions are guarded so illegal moves surface as clean 409s.
/// </summary>
public sealed class AiSource : AggregateRoot<Guid>, ISoftDeletable
{
    private readonly List<AiChunk> _chunks = [];

    public string Name { get; private set; } = default!;
    public AiSourceKind Kind { get; private set; }
    public AiSourceStatus Status { get; private set; }
    public string SourceRef { get; private set; } = default!;
    public string? MdStorageKey { get; private set; }
    public string? ErrorReason { get; private set; }

    public bool IsDeleted { get; private set; }
    public DateTimeOffset? DeletedOnUtc { get; private set; }
    public string? DeletedBy { get; private set; }

    public IReadOnlyCollection<AiChunk> Chunks => _chunks.AsReadOnly();

    private AiSource() { }

    public static AiSource CreateFileSource(string name, Guid fileAssetId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new AiSource
        {
            Id = Guid.CreateVersion7(),
            Name = name.Trim(),
            Kind = AiSourceKind.File,
            Status = AiSourceStatus.Pending,
            SourceRef = fileAssetId.ToString(),
        };
    }

    public static AiSource CreateWebSource(string name, string url)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(url);

        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new CustomException(
                $"Web source URL '{url}' is not a valid absolute http(s) URL.",
                errors: null,
                System.Net.HttpStatusCode.BadRequest);
        }

        return new AiSource
        {
            Id = Guid.CreateVersion7(),
            Name = name.Trim(),
            Kind = AiSourceKind.WebLink,
            Status = AiSourceStatus.Pending,
            SourceRef = uri.ToString(),
        };
    }

    public void AttachMarkdownTwin(string storageKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(storageKey);
        MdStorageKey = storageKey;
    }

    /// <summary>
    /// Adds one chunk to this source. Chunks are never persisted independently. Same tracking caveat
    /// as <see cref="AiChatSession.AddMessage"/>: if the source is dirtied in the same unit of work,
    /// mark the returned chunk <c>EntityState.Added</c> explicitly before saving.
    /// </summary>
    public AiChunk AddChunk(string content)
    {
        var chunk = AiChunk.Create(Id, _chunks.Count, content);
        _chunks.Add(chunk);
        return chunk;
    }

    public void MarkReady()
    {
        if (Status == AiSourceStatus.Ready)
        {
            return;
        }

        if (IsDeleted)
        {
            throw new CustomException(
                "Cannot mark a deleted source as ready.",
                errors: null,
                System.Net.HttpStatusCode.Conflict);
        }

        Status = AiSourceStatus.Ready;
        ErrorReason = null;
    }

    /// <summary>Returns the source to Pending for a refresh/retry; clears any failure reason.</summary>
    public void MarkPending()
    {
        Status = AiSourceStatus.Pending;
        ErrorReason = null;
    }

    public void MarkFailed(string reason)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);
        Status = AiSourceStatus.Failed;
        ErrorReason = reason;
    }

    public void Delete()
    {
        IsDeleted = true;
        DeletedOnUtc = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Reverses a soft delete. Idempotent.
    /// </summary>
    public void Restore()
    {
        if (!IsDeleted) return;
        IsDeleted = false;
        DeletedOnUtc = null;
        DeletedBy = null;
    }
}
