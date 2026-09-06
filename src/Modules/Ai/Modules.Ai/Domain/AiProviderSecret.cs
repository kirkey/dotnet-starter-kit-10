using FSH.Framework.Core.Domain;

namespace FSH.Modules.Ai.Domain;

/// <summary>
/// A provider credential at rest. The value is Data-Protection encrypted and is never
/// returned by any query or DTO — listings expose only presence via the provider.
/// </summary>
public sealed class AiProviderSecret : BaseEntity<Guid>
{
    public Guid ProviderId { get; private set; }
    public string KeyName { get; private set; } = default!;
    public string ProtectedValue { get; private set; } = default!;

    private AiProviderSecret() { }

    public static AiProviderSecret Create(Guid providerId, string keyName, string protectedValue)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(keyName);
        ArgumentException.ThrowIfNullOrWhiteSpace(protectedValue);

        return new AiProviderSecret
        {
            Id = Guid.CreateVersion7(),
            ProviderId = providerId,
            KeyName = keyName.Trim(),
            ProtectedValue = protectedValue,
        };
    }

    public void Replace(string protectedValue)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(protectedValue);
        ProtectedValue = protectedValue;
    }
}
