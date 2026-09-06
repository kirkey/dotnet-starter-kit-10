namespace FSH.Modules.Ai;

/// <summary>Bound from the "Ai" configuration section.</summary>
public sealed class AiOptions
{
    /// <summary>
    /// Local agent detection probes the server machine's well-known install locations.
    /// Enable only where that is meaningful (local/dev); never in shared hosting.
    /// </summary>
    public bool DetectionEnabled { get; set; }
}
