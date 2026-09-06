using FSH.Framework.Core.Domain;
using FSH.Modules.Ai.Contracts.Dtos;

namespace FSH.Modules.Ai.Domain;

/// <summary>
/// A recurring agent task. WebWatch carries {url, recipients} in TaskConfigJson; Prompt carries {prompt, recipients?}.
/// </summary>
public sealed class AiAgentSchedule : AggregateRoot<Guid>
{
    public Guid AgentId { get; private set; }
    public string Name { get; private set; } = default!;
    public string Cron { get; private set; } = default!;
    public ScheduleTaskType TaskType { get; private set; }
    public string TaskConfigJson { get; private set; } = "{}";
    public bool IsEnabled { get; private set; } = true;
    public DateTimeOffset? LastRunOnUtc { get; private set; }
    public string? LastContentHash { get; private set; }
    public string WebhookToken { get; private set; } = default!;

    private AiAgentSchedule() { }

    public static AiAgentSchedule Create(
        Guid agentId,
        string name,
        string cron,
        ScheduleTaskType taskType,
        string taskConfigJson)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(cron);
        ArgumentException.ThrowIfNullOrWhiteSpace(taskConfigJson);

        return new AiAgentSchedule
        {
            Id = Guid.CreateVersion7(),
            AgentId = agentId,
            Name = name.Trim(),
            Cron = cron.Trim(),
            TaskType = taskType,
            TaskConfigJson = taskConfigJson,
            WebhookToken = Guid.NewGuid().ToString("N"),
        };
    }

    public void Update(string name, string cron, ScheduleTaskType taskType, string taskConfigJson)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(cron);
        ArgumentException.ThrowIfNullOrWhiteSpace(taskConfigJson);
        Name = name.Trim();
        Cron = cron.Trim();
        TaskType = taskType;
        TaskConfigJson = taskConfigJson;
    }

    public void Enable() => IsEnabled = true;

    public void Disable() => IsEnabled = false;

    public void RotateWebhookToken() => WebhookToken = Guid.NewGuid().ToString("N");

    public void RecordRun(string? contentHash)
    {
        LastRunOnUtc = DateTimeOffset.UtcNow;
        LastContentHash = contentHash;
    }
}
