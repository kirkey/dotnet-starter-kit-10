using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.v1.Schedules;
using FSH.Modules.Ai.Data;
using Hangfire;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Schedules.DeleteSchedule;

public sealed class DeleteScheduleCommandHandler(AiDbContext db, IRecurringJobManager recurring)
    : ICommandHandler<DeleteScheduleCommand, Guid>
{
    public async ValueTask<Guid> Handle(DeleteScheduleCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var schedule = await db.AgentSchedules
            .FirstOrDefaultAsync(s => s.Id == command.Id, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Schedule {command.Id} was not found.");

        ScheduleSupport.RemoveRecurring(recurring, schedule.Id);

        db.AgentSchedules.Remove(schedule);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return schedule.Id;
    }
}
