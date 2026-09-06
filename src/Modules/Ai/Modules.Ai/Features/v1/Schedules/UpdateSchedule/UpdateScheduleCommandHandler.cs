using System.Net;
using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.v1.Schedules;
using FSH.Modules.Ai.Data;
using Hangfire;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Schedules.UpdateSchedule;

public sealed class UpdateScheduleCommandHandler(
    AiDbContext db,
    IRecurringJobManager recurring,
    ICurrentUser currentUser)
    : ICommandHandler<UpdateScheduleCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateScheduleCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var tenantId = currentUser.GetTenant() ?? throw new UnauthorizedException("invalid tenant");

        var schedule = await db.AgentSchedules
            .FirstOrDefaultAsync(s => s.Id == command.Id, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Schedule {command.Id} was not found.");

        schedule.Update(
            command.Name,
            command.Cron,
            command.TaskType,
            ScheduleSupport.BuildConfigJson(command.Url, command.Prompt, command.Recipients));
        if (command.IsEnabled)
        {
            schedule.Enable();
        }
        else
        {
            schedule.Disable();
        }

        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            ScheduleSupport.RegisterRecurring(recurring, tenantId, schedule);
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
        {
            throw new CustomException(
                $"Cron expression '{command.Cron}' was rejected by the scheduler: {ex.Message}",
                errors: null,
                HttpStatusCode.BadRequest);
        }

        return schedule.Id;
    }
}
