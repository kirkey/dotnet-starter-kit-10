namespace FSH.Modules.Ai.Contracts.Dtos;

/// <summary>Lifecycle state of an agent run.</summary>
public enum AgentRunStatus
{
    Queued,
    Running,
    Completed,
    Failed,
    Cancelled
}
