namespace FSH.Modules.Ai.Contracts.Dtos;

public sealed record AiProviderModelDto(
    string ModelId,
    string DisplayName,
    bool SupportsChat,
    bool SupportsEmbeddings);

public sealed record AiProviderDto(
    Guid Id,
    string Name,
    AiProviderType ProviderType,
    string? BaseUrl,
    string? ChatModel,
    string? EmbeddingModel,
    int EmbeddingDimensions,
    bool IsDefaultChat,
    bool IsDefaultEmbedding,
    int Revision,
    bool HasApiKey,
    IReadOnlyList<AiProviderModelDto> Models);
