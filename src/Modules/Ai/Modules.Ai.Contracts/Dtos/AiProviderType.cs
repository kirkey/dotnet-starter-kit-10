namespace FSH.Modules.Ai.Contracts.Dtos;

/// <summary>Provider backend type. Local is the built-in deterministic provider; OpenAiCompatible covers OpenAI and compatible endpoints.</summary>
public enum AiProviderType
{
    Local,
    OpenAiCompatible
}
