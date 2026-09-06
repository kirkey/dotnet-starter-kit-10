using FSH.Framework.Core.Domain;
using FSH.Modules.Ai.Contracts.Dtos;

namespace FSH.Modules.Ai.Domain;

/// <summary>
/// A chat session: an ordered message thread bound to a model + variant snapshot (taken from
/// the chosen agent target at creation, defaulting to the tenant default). AgentId is a loose
/// reference so sessions survive agent archival.
/// </summary>
public sealed class AiChatSession : AggregateRoot<Guid>
{
    private readonly List<AiChatMessage> _messages = [];

    public string Title { get; private set; } = default!;
    public string Model { get; private set; } = default!;
    public AiVariant Variant { get; private set; }
    public Guid? AgentId { get; private set; }
    public Guid OwnerId { get; private set; }
    public DateTimeOffset LastActivityUtc { get; private set; }
    public int MessageCount { get; private set; }

    public IReadOnlyCollection<AiChatMessage> Messages => _messages.AsReadOnly();

    private AiChatSession() { }

    public static AiChatSession Create(string title, string model, AiVariant variant, Guid? agentId, Guid ownerId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(model);

        return new AiChatSession
        {
            Id = Guid.CreateVersion7(),
            Title = title.Trim(),
            Model = model.Trim(),
            Variant = variant,
            AgentId = agentId,
            OwnerId = ownerId,
            LastActivityUtc = DateTimeOffset.UtcNow,
        };
    }

    /// <summary>
    /// Adds a message and refreshes counters/activity. Callers persisting this in the same unit of
    /// work as the add MUST mark the returned message <c>EntityState.Added</c> explicitly: adding to
    /// the collection of an already-dirtied parent mis-tracks the row as Modified through
    /// DetectChanges (0-row concurrency failure on save).
    /// </summary>
    public AiChatMessage AddMessage(string role, string content, IReadOnlyList<string> citedSources)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(role);
        ArgumentException.ThrowIfNullOrWhiteSpace(content);
        ArgumentNullException.ThrowIfNull(citedSources);

        var message = AiChatMessage.Create(Id, _messages.Count, role.Trim(), content, citedSources);
        _messages.Add(message);
        MessageCount = _messages.Count;
        LastActivityUtc = DateTimeOffset.UtcNow;
        return message;
    }

    public void Rename(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        Title = title.Trim();
        LastActivityUtc = DateTimeOffset.UtcNow;
    }
}
