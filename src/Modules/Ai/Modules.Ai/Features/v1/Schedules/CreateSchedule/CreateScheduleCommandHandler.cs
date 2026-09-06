using System.Net;
using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.v1.Schedules;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Domain;
using Hangfire;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Schedules.CreateSchedule;

public sealed class CreateScheduleCommandHandler(
    AiDbContext db,
    IRecurringJobManager recurring,
    ICurrentUser currentUser)
    : ICommandHandler<CreateScheduleCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateScheduleCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var tenantId = currentUser.GetTenant() ?? throw new UnauthorizedException("invalid tenant");

        var agentExists = await db.Agents
            .AnyAsync(a => a.Id == command.AgentId, cancellationToken)
            .ConfigureAwait(false);
        if (!agentExists)
        {
            throw new NotFoundException($"Agent {command.AgentId} was not found.");
        }

        var schedule = AiAgentSchedule.Create(
            command.AgentId,
            command.Name,
            command.Cron,
            command.TaskType,
            ScheduleSupport.BuildConfigJson(command.Url, command.Prompt, command.Recipients));

        db.AgentSchedules.Add(schedule);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            ScheduleSupport.RegisterRecurring(recurring, tenantId, schedule);
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
        {
            db.AgentSchedules.Remove(schedule);
            await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            throw new CustomException(
                $"Cron expression '{command.Cron}' was rejected by the scheduler: {ex.Message}",
                errors: null,
                HttpStatusCode.BadRequest);
        }

        return schedule.Id;
    }
}
