using FSH.Modules.Ai.Contracts.Dtos;

namespace FSH.Modules.Ai.Services;

/// <summary>
/// Declares which effort tiers each known runtime family supports. Unknown families allow
/// Default only — anything else is rejected at agent save time, never at run time.
/// </summary>
public static class RuntimeVariantMatrix
{
    private static readonly IReadOnlyDictionary<string, IReadOnlyList<AiVariant>> Supported =
        new Dictionary<string, IReadOnlyList<AiVariant>>(StringComparer.OrdinalIgnoreCase)
        {
            ["claude-code"] = [AiVariant.Default, AiVariant.Normal, AiVariant.High, AiVariant.ExtraHigh],
            ["opencode"] = [AiVariant.Default, AiVariant.Normal, AiVariant.High, AiVariant.ExtraHigh],
            ["codex"] = [AiVariant.Default, AiVariant.Normal, AiVariant.High],
            ["cursor"] = [AiVariant.Default, AiVariant.Normal, AiVariant.High],
            ["gemini-cli"] = [AiVariant.Default, AiVariant.Normal],
            ["local-model"] = [AiVariant.Default],
        };

    public static IReadOnlyList<AiVariant> SupportedTiers(string family) =>
        string.IsNullOrWhiteSpace(family) || !Supported.TryGetValue(family.Trim(), out var tiers)
            ? [AiVariant.Default]
            : tiers;

    public static bool IsSupported(string family, AiVariant variant) =>
        SupportedTiers(family).Contains(variant);

    public static bool IsKnownFamily(string family) =>
        !string.IsNullOrWhiteSpace(family) && Supported.ContainsKey(family.Trim());

    public static IReadOnlyList<string> KnownFamilyNames() =>
        Supported.Keys.OrderBy(f => f, StringComparer.Ordinal).ToList();

    public static string DisplayName(string family)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(family);
        return family.Trim().ToUpperInvariant() switch
        {
            "CLAUDE-CODE" => "Claude Code",
            "OPENCODE" => "opencode",
            "CODEX" => "OpenAI Codex",
            "CURSOR" => "Cursor",
            "GEMINI-CLI" => "Gemini CLI",
            "LOCAL-MODEL" => "Local model",
            _ => family.Trim(),
        };
    }
}
