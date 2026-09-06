using FSH.Modules.Ai.Contracts.Dtos;
using Mediator;

namespace FSH.Modules.Ai.Contracts.v1.Providers;

public sealed record ProviderModelInput(
    string ModelId,
    string DisplayName,
    bool SupportsChat,
    bool SupportsEmbeddings);

public sealed record CreateProviderCommand(
    string Name,
    AiProviderType ProviderType,
    string? BaseUrl,
    string? ChatModel,
    string? EmbeddingModel,
    int EmbeddingDimensions,
    IReadOnlyList<ProviderModelInput> Models) : ICommand<Guid>;
