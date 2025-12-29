namespace FSH.Framework.Shared.Auditing;

/// <summary>Marks a property that should be excluded from audit diffs and payloads.</summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class AuditIgnoreAttribute : Attribute { }

/// <summary>
/// Marks a property as sensitive (to be masked or hashed when serialized).
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class AuditSensitiveAttribute(bool hash = false, bool redact = false) : Attribute
{
    public bool Hash { get; init; } = hash;
    public bool Redact { get; init; } = redact;
}

