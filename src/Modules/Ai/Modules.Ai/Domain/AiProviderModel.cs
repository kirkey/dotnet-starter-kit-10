using FSH.Framework.Core.Domain;

namespace FSH.Modules.Ai.Domain;

/// <summary>One model served by a provider, with per-capability flags.</summary>
public sealed class AiProviderModel : BaseEntity<Guid>
{
    public Guid ProviderId { get; private set; }
    public string ModelId { get; private set; } = default!;
    public string DisplayName { get; private set; } = default!;
    public bool SupportsChat { get; private set; }
    public bool SupportsEmbeddings { get; private set; }

    private AiProviderModel() { }

    public static AiProviderModel Create(
        Guid providerId,
        string modelId,
        string displayName,
        bool supportsChat,
        bool supportsEmbeddings)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(modelId);
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);
        if (!supportsChat && !supportsEmbeddings)
        {
            throw new ArgumentException("A model must support at least one capability.", nameof(supportsChat));
        }

        return new AiProviderModel
        {
            Id = Guid.CreateVersion7(),
            ProviderId = providerId,
            ModelId = modelId.Trim(),
            DisplayName = displayName.Trim(),
            SupportsChat = supportsChat,
            SupportsEmbeddings = supportsEmbeddings,
        };
    }
}
