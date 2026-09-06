using Mediator;

namespace FSH.Modules.Ai.Contracts.v1.Providers;

public sealed record UpdateProviderCommand(
    Guid Id,
    int Revision,
    string Name,
    string? BaseUrl,
    string? ChatModel,
    string? EmbeddingModel,
    int EmbeddingDimensions,
    IReadOnlyList<ProviderModelInput> Models) : ICommand<Guid>;
