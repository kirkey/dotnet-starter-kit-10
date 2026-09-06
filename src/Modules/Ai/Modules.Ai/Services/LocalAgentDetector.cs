namespace FSH.Modules.Ai.Services;

public sealed record DetectedAgent(
    string Family,
    string DisplayName,
    string? DetectedVersion,
    bool HasCredentials,
    string EvidencePath);

public interface ILocalAgentDetector
{
    IReadOnlyList<DetectedAgent> Detect();
}

/// <summary>
/// Detects AI coding agents installed on this machine by inspecting well-known install
/// locations only. Never executes agent binaries; credential presence is reported by file
/// existence — values are never read, logged, or transmitted.
/// </summary>
public sealed class LocalAgentDetector : ILocalAgentDetector
{
    private readonly string _home;

    /// <param name="home">Override for the probed home directory (test seam); defaults to the user profile.</param>
    public LocalAgentDetector(string? home = null) => _home = home
        ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

    public IReadOnlyList<DetectedAgent> Detect()
    {
        if (string.IsNullOrWhiteSpace(_home) || !Directory.Exists(_home))
        {
            return [];
        }

        return Probes()
            .Where(p => Directory.Exists(Path.Combine(_home, p.ConfigDir)))
            .Select(p => new DetectedAgent(
                p.Family,
                p.DisplayName,
                ReadVersion(p),
                p.CredentialFiles.Any(f => File.Exists(Path.Combine(_home, p.ConfigDir, f))),
                Path.Combine(_home, p.ConfigDir)))
            .ToList();
    }

    private string? ReadVersion(Probe probe)
    {
        // Metadata files only (e.g. a VERSION marker); config/credential files are never parsed.
        if (probe.VersionFile is null)
        {
            return null;
        }

        var path = Path.Combine(_home, probe.ConfigDir, probe.VersionFile);
        if (!File.Exists(path))
        {
            return null;
        }

        try
        {
            var text = File.ReadAllText(path).Trim();
            return text.Length is > 0 and <= 32 ? text : null;
        }
        catch (IOException)
        {
            return null;
        }
        catch (UnauthorizedAccessException)
        {
            return null;
        }
    }

    private sealed record Probe(
        string Family,
        string DisplayName,
        string ConfigDir,
        string[] CredentialFiles,
        string? VersionFile = null);

    private static IReadOnlyList<Probe> Probes() =>
    [
        new("claude-code", "Claude Code", ".claude", [".credentials.json"], "VERSION"),
        new("codex", "OpenAI Codex", ".codex", ["auth.json"], "VERSION"),
        new("opencode", "opencode", ".config/opencode", ["auth.json"], "VERSION"),
        new("gemini-cli", "Gemini CLI", ".gemini", ["oauth_creds.json"], "VERSION"),
        new("cursor", "Cursor", ".cursor", ["auth.json"], "VERSION"),
    ];
}
