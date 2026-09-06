using FSH.Modules.Ai.Contracts.Dtos;
using Mediator;

namespace FSH.Modules.Ai.Contracts.v1.Runtimes;

public sealed record ListRuntimesQuery : IQuery<IReadOnlyList<AiRuntimeDto>>;

public sealed record RefreshRuntimeCatalogCommand : ICommand<IReadOnlyList<AiRuntimeDto>>;
