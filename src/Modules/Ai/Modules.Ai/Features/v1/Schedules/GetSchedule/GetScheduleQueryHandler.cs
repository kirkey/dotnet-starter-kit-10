using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Schedules;
using FSH.Modules.Ai.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Schedules.GetSchedule;

public sealed class GetScheduleQueryHandler(AiDbContext db)
    : IQueryHandler<GetScheduleQuery, ScheduleDetailDto>
{
    public async ValueTask<ScheduleDetailDto> Handle(GetScheduleQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var schedule = await db.AgentSchedules
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == query.Id, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Schedule {query.Id} was not found.");

        return new ScheduleDetailDto(
            schedule.Id, schedule.AgentId, schedule.Name, schedule.Cron, schedule.TaskType,
            schedule.IsEnabled, schedule.TaskConfigJson, schedule.WebhookToken, schedule.LastRunOnUtc);
    }
}
