namespace FSH.Modules.Ai.Contracts.Dtos;

public sealed record AiDepartmentDto(
    Guid Id,
    string Name,
    string? Description,
    int AgentCount);

public sealed record AiAgentDto(
    Guid Id,
    Guid DepartmentId,
    string Name,
    string Instructions,
    IReadOnlyList<string> Skills,
    string RuntimeBinding,
    string Model,
    AiVariant Variant,
    AgentAccessMode AccessMode,
    IReadOnlyList<string> AccessUserIds,
    bool IsArchived);
