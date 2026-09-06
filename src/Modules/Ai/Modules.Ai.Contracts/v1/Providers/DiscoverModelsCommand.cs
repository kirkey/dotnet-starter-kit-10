using FSH.Modules.Ai.Contracts.Dtos;
using Mediator;

namespace FSH.Modules.Ai.Contracts.v1.Providers;

public sealed record DiscoverModelsCommand(string BaseUrl, string? ApiKey)
    : ICommand<IReadOnlyList<DiscoveredModelDto>>;
