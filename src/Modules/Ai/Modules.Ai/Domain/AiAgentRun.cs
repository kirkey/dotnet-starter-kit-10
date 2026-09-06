using FSH.Framework.Core.Domain;
using FSH.Modules.Ai.Contracts.Dtos;

namespace FSH.Modules.Ai.Domain;

/// <summary>One tracked agent run: queued → running → completed/failed (or cancelled).</summary>
public sealed class AiAgentRun : AggregateRoot<Guid>
{
    public Guid AgentId { get; private set; }
    public Guid? ScheduleId { get; private set; }
    public AgentRunTrigger Trigger { get; private set; }
    public AgentRunStatus Status { get; private set; }
    public string Input { get; private set; } = default!;
    public string? Output { get; private set; }
    public string? Error { get; private set; }
    public int RetryCount { get; private set; }
    public DateTimeOffset? StartedOnUtc { get; private set; }
    public DateTimeOffset? FinishedOnUtc { get; private set; }

    private AiAgentRun() { }

    public static AiAgentRun Create(Guid agentId, Guid? scheduleId, AgentRunTrigger trigger, string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);

        return new AiAgentRun
        {
            Id = Guid.CreateVersion7(),
            AgentId = agentId,
            ScheduleId = scheduleId,
            Trigger = trigger,
            Status = AgentRunStatus.Queued,
            Input = input.Trim(),
        };
    }

    public void MarkRunning()
    {
        Status = AgentRunStatus.Running;
        StartedOnUtc = DateTimeOffset.UtcNow;
    }

    public void MarkCompleted(string output)
    {
        Status = AgentRunStatus.Completed;
        Output = output;
        FinishedOnUtc = DateTimeOffset.UtcNow;
    }

    public void MarkFailed(string error)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(error);
        Status = AgentRunStatus.Failed;
        Error = error;
        FinishedOnUtc = DateTimeOffset.UtcNow;
    }

    public void MarkCancelled()
    {
        Status = AgentRunStatus.Cancelled;
        FinishedOnUtc = DateTimeOffset.UtcNow;
    }

    public void RecordRetry() => RetryCount++;
}
