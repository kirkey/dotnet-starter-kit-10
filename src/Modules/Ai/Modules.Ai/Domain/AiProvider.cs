using FSH.Framework.Core.Domain;
using FSH.Modules.Ai.Contracts.Dtos;

namespace FSH.Modules.Ai.Domain;

/// <summary>
/// Configured AI provider: backend type, endpoint, models served, and which tenant
/// default each capability resolves to. Secrets live in <see cref="AiProviderSecret"/>
/// (Data-Protection encrypted, never returned by any query). Writes are revision-guarded:
/// callers pass the last-seen revision and stale writes fail with 409.
/// </summary>
public sealed class AiProvider : AggregateRoot<Guid>
{
    private readonly List<AiProviderModel> _models = [];

    public string Name { get; private set; } = default!;
    public AiProviderType ProviderType { get; private set; }
    public string? BaseUrl { get; private set; }
    public string? ChatModel { get; private set; }
    public string? EmbeddingModel { get; private set; }
    public int EmbeddingDimensions { get; private set; }
    public bool IsDefaultChat { get; private set; }
    public bool IsDefaultEmbedding { get; private set; }
    public int Revision { get; private set; } = 1;

    public IReadOnlyCollection<AiProviderModel> Models => _models.AsReadOnly();

    private AiProvider() { }

    public static AiProvider Create(
        string name,
        AiProviderType providerType,
        string? baseUrl,
        string? chatModel,
        string? embeddingModel,
        int embeddingDimensions)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (embeddingDimensions is < 64 or > AiChunk.EmbeddingDimensions)
        {
            throw new ArgumentOutOfRangeException(
                nameof(embeddingDimensions),
                $"Embedding dimensions must be between 64 and {AiChunk.EmbeddingDimensions}.");
        }

        return new AiProvider
        {
            Id = Guid.CreateVersion7(),
            Name = name.Trim(),
            ProviderType = providerType,
            BaseUrl = string.IsNullOrWhiteSpace(baseUrl) ? null : baseUrl.Trim(),
            ChatModel = string.IsNullOrWhiteSpace(chatModel) ? null : chatModel.Trim(),
            EmbeddingModel = string.IsNullOrWhiteSpace(embeddingModel) ? null : embeddingModel.Trim(),
            EmbeddingDimensions = embeddingDimensions,
        };
    }

    public void Update(
        string name,
        string? baseUrl,
        string? chatModel,
        string? embeddingModel,
        int embeddingDimensions)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name.Trim();
        BaseUrl = string.IsNullOrWhiteSpace(baseUrl) ? null : baseUrl.Trim();
        ChatModel = string.IsNullOrWhiteSpace(chatModel) ? null : chatModel.Trim();
        EmbeddingModel = string.IsNullOrWhiteSpace(embeddingModel) ? null : embeddingModel.Trim();
        EmbeddingDimensions = embeddingDimensions;
        Revision++;
    }

    /// <summary>
    /// Replaces the model catalog. Only safe on unattached (new) aggregates: on an already-tracked
    /// parent the Clear()+AddRange on the navigation mis-tracks replacements — manage rows via the
    /// DbSet (bulk delete + AddRange) instead.
    /// </summary>
    public void SetModels(IEnumerable<AiProviderModel> models)
    {
        ArgumentNullException.ThrowIfNull(models);
        _models.Clear();
        _models.AddRange(models);
    }

    public void SetAsDefault(bool chat, bool embedding)
    {
        IsDefaultChat = chat;
        IsDefaultEmbedding = embedding;
        Revision++;
    }

    public void ClearDefault(bool chat, bool embedding)
    {
        if (chat)
        {
            IsDefaultChat = false;
        }

        if (embedding)
        {
            IsDefaultEmbedding = false;
        }

        Revision++;
    }
}
