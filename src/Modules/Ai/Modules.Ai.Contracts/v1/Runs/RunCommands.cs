using FSH.Modules.Ai.Contracts.Dtos;
using Mediator;

namespace FSH.Modules.Ai.Contracts.v1.Runs;

public sealed record StartAgentRunCommand(Guid AgentId, string Input) : ICommand<Guid>;

public sealed record ListAgentRunsQuery(Guid? AgentId, AgentRunStatus? Status) : IQuery<IReadOnlyList<AiAgentRunDto>>;

public sealed record GetAgentRunQuery(Guid Id) : IQuery<AiAgentRunDto>;

public sealed record EndAgentRunCommand(Guid Id) : ICommand<Guid>;
