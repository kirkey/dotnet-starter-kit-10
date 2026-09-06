using System.Text.RegularExpressions;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Domain;
using FSH.Modules.Ai.Jobs;
using Hangfire;

namespace FSH.Modules.Ai.Features.v1.Schedules;

internal static partial class ScheduleSupport
{
    public static string RecurringKey(Guid scheduleId) => $"ai-schedule-{scheduleId:N}";

    public static bool HasCronShape(string cron) => CronShapeRegex().IsMatch(cron.Trim());

    public static string BuildConfigJson(string? url, string? prompt, IReadOnlyList<string> recipients)
    {
        var safeRecipients = recipients
            .Where(r => !string.IsNullOrWhiteSpace(r))
            .Select(r => r.Trim())
            .ToList();
        return System.Text.Json.JsonSerializer.Serialize(new
        {
            url = string.IsNullOrWhiteSpace(url) ? null : url.Trim(),
            prompt = string.IsNullOrWhiteSpace(prompt) ? null : prompt.Trim(),
            recipients = safeRecipients,
        });
    }

    public static void RegisterRecurring(IRecurringJobManager recurring, string tenantId, AiAgentSchedule schedule)
    {
        ArgumentNullException.ThrowIfNull(recurring);
        ArgumentNullException.ThrowIfNull(schedule);

        if (!schedule.IsEnabled)
        {
            recurring.RemoveIfExists(RecurringKey(schedule.Id));
            return;
        }

        recurring.AddOrUpdate<Jobs.AgentScheduleJob>(
            RecurringKey(schedule.Id),
            j => j.TriggerAsync(tenantId, schedule.Id, CancellationToken.None),
            schedule.Cron,
            new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });
    }

    public static void RemoveRecurring(IRecurringJobManager recurring, Guid scheduleId)
    {
        ArgumentNullException.ThrowIfNull(recurring);
        recurring.RemoveIfExists(RecurringKey(scheduleId));
    }

    [GeneratedRegex(@"^(\S+\s+){4}\S+$")]
    private static partial Regex CronShapeRegex();
}
