using FSH.Modules.Ai.Contracts.Dtos;
using Mediator;

namespace FSH.Modules.Ai.Contracts.v1.Schedules;

public sealed record CreateScheduleCommand(
    Guid AgentId,
    string Name,
    string Cron,
    ScheduleTaskType TaskType,
    string? Url,
    string? Prompt,
    IReadOnlyList<string> Recipients) : ICommand<Guid>;

public sealed record UpdateScheduleCommand(
    Guid Id,
    string Name,
    string Cron,
    ScheduleTaskType TaskType,
    bool IsEnabled,
    string? Url,
    string? Prompt,
    IReadOnlyList<string> Recipients) : ICommand<Guid>;

public sealed record DeleteScheduleCommand(Guid Id) : ICommand<Guid>;

public sealed record ListSchedulesQuery(Guid? AgentId) : IQuery<IReadOnlyList<AiAgentScheduleDto>>;

public sealed record GetScheduleQuery(Guid Id) : IQuery<ScheduleDetailDto>;

public sealed record SetScheduleWebhookCommand(Guid Id) : ICommand<string>;

public sealed record StartScheduledRunCommand(Guid ScheduleId, string Token) : ICommand<Guid>;
