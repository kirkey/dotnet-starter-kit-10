using FSH.Modules.Ai.Contracts.Dtos;
using Mediator;

namespace FSH.Modules.Ai.Contracts.v1.Providers;

public sealed record GetProviderQuery(Guid Id) : IQuery<AiProviderDto>;

public sealed record ListProvidersQuery : IQuery<IReadOnlyList<AiProviderDto>>;
