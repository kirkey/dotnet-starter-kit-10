using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Jobs.Services;
using FSH.Framework.Shared.Multitenancy;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Modules.Ai.Jobs;

/// <summary>
/// Hangfire-recurring entry point per schedule: materializes one run row and enqueues execution.
/// Quiet by design — a missing/disabled schedule or archived agent just logs and returns.
/// </summary>
public sealed class AgentScheduleJob(
    IJobService jobs,
    IServiceScopeFactory scopeFactory,
    ILogger<AgentScheduleJob> logger)
{
    public async Task TriggerAsync(string tenantId, Guid scheduleId, CancellationToken ct)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantId);

        using var scope = scopeFactory.CreateScope();
        var tenant = await scope.ServiceProvider
            .GetRequiredService<IMultiTenantStore<AppTenantInfo>>()
            .GetAsync(tenantId)
            .ConfigureAwait(false);
        if (tenant is null)
        {
            return;
        }

        scope.ServiceProvider.GetRequiredService<IMultiTenantContextSetter>().MultiTenantContext =
            new MultiTenantContext<AppTenantInfo>(tenant);

        var db = scope.ServiceProvider.GetRequiredService<AiDbContext>();
        var schedule = await db.AgentSchedules
            .FirstOrDefaultAsync(s => s.Id == scheduleId, ct)
            .ConfigureAwait(false);
        if (schedule is null || !schedule.IsEnabled)
        {
            return;
        }

        var agent = await db.Agents
            .FirstOrDefaultAsync(a => a.Id == schedule.AgentId, ct)
            .ConfigureAwait(false);
        if (agent is null || agent.IsArchived)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(
                    "[Ai] schedule {ScheduleId} skipped: agent missing or archived", scheduleId);
            }

            return;
        }

        var run = AiAgentRun.Create(
            agent.Id,
            schedule.Id,
            AgentRunTrigger.Schedule,
            schedule.TaskType == ScheduleTaskType.WebWatch
                ? $"Scheduled web-watch: {schedule.Name}"
                : $"Scheduled run: {schedule.Name}");
        db.AgentRuns.Add(run);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);

        jobs.Enqueue<AgentRunJob>(j => j.ExecuteAsync(tenantId, run.Id, CancellationToken.None));

        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("[Ai] schedule {ScheduleId} queued run {RunId}", scheduleId, run.Id);
        }
    }
}
