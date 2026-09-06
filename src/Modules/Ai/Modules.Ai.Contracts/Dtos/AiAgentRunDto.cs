namespace FSH.Modules.Ai.Contracts.Dtos;

public sealed record AiAgentRunDto(
    Guid Id,
    Guid AgentId,
    AgentRunTrigger Trigger,
    AgentRunStatus Status,
    string Input,
    string? Output,
    string? Error,
    int RetryCount,
    DateTimeOffset? StartedOnUtc,
    DateTimeOffset? FinishedOnUtc);

public sealed record AiAgentScheduleDto(
    Guid Id,
    Guid AgentId,
    string Name,
    string Cron,
    ScheduleTaskType TaskType,
    bool IsEnabled,
    DateTimeOffset? LastRunOnUtc);

public sealed record AiRuntimeDto(
    Guid Id,
    string Family,
    string DisplayName,
    string? DetectedVersion,
    bool IsOnline,
    DateTimeOffset? LastSeenOnUtc);
