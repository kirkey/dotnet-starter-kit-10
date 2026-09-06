using FSH.Modules.Ai.Contracts.Dtos;
using Mediator;

namespace FSH.Modules.Ai.Contracts.v1.Agents;

public sealed record CreateAgentCommand(
    Guid DepartmentId,
    string Name,
    string Instructions,
    IReadOnlyList<string> Skills,
    string RuntimeBinding,
    string Model,
    AiVariant Variant,
    AgentAccessMode AccessMode,
    IReadOnlyList<string> AccessUserIds) : ICommand<Guid>;

public sealed record UpdateAgentCommand(
    Guid Id,
    string Name,
    string Instructions,
    IReadOnlyList<string> Skills,
    string RuntimeBinding,
    string Model,
    AiVariant Variant,
    AgentAccessMode AccessMode,
    IReadOnlyList<string> AccessUserIds) : ICommand<Guid>;

public sealed record GetAgentQuery(Guid Id) : IQuery<AiAgentDto>;

public sealed record ListAgentsQuery(Guid? DepartmentId, bool IncludeArchived) : IQuery<IReadOnlyList<AiAgentDto>>;

public sealed record ArchiveAgentCommand(Guid Id) : ICommand<Guid>;

public sealed record RestoreAgentCommand(Guid Id) : ICommand<Guid>;

public sealed record CreateAgentCopyCommand(Guid Id, string Name) : ICommand<Guid>;
