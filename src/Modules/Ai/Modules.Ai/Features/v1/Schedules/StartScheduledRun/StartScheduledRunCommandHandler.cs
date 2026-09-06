using System.Net;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Jobs.Services;
using FSH.Framework.Shared.Multitenancy;
using FSH.Modules.Ai.Contracts.v1.Schedules;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Domain;
using FSH.Modules.Ai.Jobs;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Modules.Ai.Features.v1.Schedules.StartScheduledRun;

public sealed class StartScheduledRunCommandHandler(
    AiDbContext outerDb,
    IJobService jobs,
    IServiceScopeFactory scopeFactory)
    : ICommandHandler<StartScheduledRunCommand, Guid>
{
    public async ValueTask<Guid> Handle(StartScheduledRunCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        // Token-authenticated webhooks carry no user and often no tenant header. The outer
        // DbContext was resolved before any tenant existed, so locate the schedule unfiltered,
        // install its tenant in a fresh scope, and do all filtered work with a fresh DbContext
        // (same construction-order rule as the Hangfire jobs).
        var schedule = await outerDb.AgentSchedules
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(
                s => s.Id == command.ScheduleId && s.WebhookToken == command.Token, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Schedule {command.ScheduleId} was not found.");

        var tenantId = outerDb.Entry(schedule).Property<string>("TenantId").CurrentValue;
        if (string.IsNullOrWhiteSpace(tenantId))
        {
            throw new UnauthorizedException("invalid tenant");
        }

        using var scope = scopeFactory.CreateScope();
        var tenant = await scope.ServiceProvider
            .GetRequiredService<IMultiTenantStore<AppTenantInfo>>()
            .GetAsync(tenantId)
            .ConfigureAwait(false)
            ?? throw new UnauthorizedException("invalid tenant");

        scope.ServiceProvider.GetRequiredService<IMultiTenantContextSetter>().MultiTenantContext =
            new MultiTenantContext<AppTenantInfo>(tenant);

        var db = scope.ServiceProvider.GetRequiredService<AiDbContext>();
        return await HandleInnerAsync(db, jobs, schedule.Id, cancellationToken).ConfigureAwait(false);
    }

    private static async Task<Guid> HandleInnerAsync(
        AiDbContext db, IJobService jobs, Guid scheduleId, CancellationToken cancellationToken)
    {
        var schedule = await db.AgentSchedules
            .FirstOrDefaultAsync(s => s.Id == scheduleId, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Schedule {scheduleId} was not found.");

        if (!schedule.IsEnabled)
        {
            throw new CustomException(
                $"Schedule '{schedule.Name}' is disabled.",
                errors: null,
                HttpStatusCode.Conflict);
        }

        var agent = await db.Agents
            .FirstOrDefaultAsync(a => a.Id == schedule.AgentId, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Agent {schedule.AgentId} was not found.");

        if (agent.IsArchived)
        {
            throw new CustomException(
                $"Agent '{agent.Name}' is archived and takes no runs.",
                errors: null,
                HttpStatusCode.Conflict);
        }

        var tenantId = db.Entry(schedule).Property<string>("TenantId").CurrentValue
            ?? throw new UnauthorizedException("invalid tenant");

        var run = AiAgentRun.Create(agent.Id, schedule.Id, Contracts.Dtos.AgentRunTrigger.Webhook, $"Webhook trigger: {schedule.Name}");
        db.AgentRuns.Add(run);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        jobs.Enqueue<AgentRunJob>(j => j.ExecuteAsync(tenantId, run.Id, CancellationToken.None));

        return run.Id;
    }
}
