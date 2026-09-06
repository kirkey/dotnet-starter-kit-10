namespace FSH.Modules.Ai.Contracts.Dtos;

/// <summary>Effort tier for a model run. Which tiers a runtime supports is declared by the runtime matrix; unsupported pairs are rejected at save time.</summary>
public enum AiVariant
{
    Default,
    Normal,
    High,
    ExtraHigh
}
