namespace FSH.Modules.Ai.Contracts.Dtos;

/// <summary>Who may run an agent: every department member, or only the listed users.</summary>
public enum AgentAccessMode
{
    Department,
    Selected
}
