using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Schedules;
using FSH.Modules.Ai.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Schedules.ListSchedules;

public sealed class ListSchedulesQueryHandler(AiDbContext db)
    : IQueryHandler<ListSchedulesQuery, IReadOnlyList<AiAgentScheduleDto>>
{
    public async ValueTask<IReadOnlyList<AiAgentScheduleDto>> Handle(ListSchedulesQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var q = db.AgentSchedules.AsNoTracking().AsQueryable();
        if (query.AgentId.HasValue)
        {
            q = q.Where(s => s.AgentId == query.AgentId.Value);
        }

        return await q
            .OrderBy(s => s.Name)
            .Select(s => new AiAgentScheduleDto(
                s.Id, s.AgentId, s.Name, s.Cron, s.TaskType, s.IsEnabled, s.LastRunOnUtc))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
