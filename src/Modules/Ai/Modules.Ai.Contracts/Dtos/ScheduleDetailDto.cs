namespace FSH.Modules.Ai.Contracts.Dtos;

public sealed record ScheduleDetailDto(
    Guid Id,
    Guid AgentId,
    string Name,
    string Cron,
    ScheduleTaskType TaskType,
    bool IsEnabled,
    string TaskConfigJson,
    string WebhookToken,
    DateTimeOffset? LastRunOnUtc);
