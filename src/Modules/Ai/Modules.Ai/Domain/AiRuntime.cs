using FSH.Framework.Core.Domain;

namespace FSH.Modules.Ai.Domain;

/// <summary>
/// One known agent-CLI family: the stable directory agent-to-runtime bindings reference.
/// Detection reconciles entries (version, online/offline); agents and history survive flips.
/// </summary>
public sealed class AiRuntime : BaseEntity<Guid>
{
    public string Family { get; private set; } = default!;
    public string DisplayName { get; private set; } = default!;
    public string? DetectedVersion { get; private set; }
    public string Source { get; private set; } = "manual";
    public bool IsOnline { get; private set; }
    public DateTimeOffset? LastSeenOnUtc { get; private set; }

    private AiRuntime() { }

    public static AiRuntime Register(string family, string displayName, string source = "manual")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(family);
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);

        return new AiRuntime
        {
            Id = Guid.CreateVersion7(),
            Family = family.Trim(),
            DisplayName = displayName.Trim(),
            Source = source,
        };
    }

    public void ReportSeen(string? version)
    {
        IsOnline = true;
        LastSeenOnUtc = DateTimeOffset.UtcNow;
        if (!string.IsNullOrWhiteSpace(version))
        {
            DetectedVersion = version.Trim();
        }

        Source = "detection";
    }

    public void ReportMissing()
    {
        IsOnline = false;
    }
}
