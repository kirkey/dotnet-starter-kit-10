using System.Text.Json;
using FSH.Framework.Core.Domain;

namespace FSH.Modules.Ai.Domain;

/// <summary>One message in a chat session. Citations persist as a JSON array of source names.</summary>
public sealed class AiChatMessage : BaseEntity<Guid>
{
    public Guid SessionId { get; private set; }
    public int Ordinal { get; private set; }
    public string Role { get; private set; } = default!;
    public string Content { get; private set; } = default!;
    public string CitedSourcesJson { get; private set; } = "[]";

    private AiChatMessage() { }

    internal static AiChatMessage Create(
        Guid sessionId,
        int ordinal,
        string role,
        string content,
        IReadOnlyList<string> citedSources)
    {
        return new AiChatMessage
        {
            Id = Guid.CreateVersion7(),
            SessionId = sessionId,
            Ordinal = ordinal,
            Role = role,
            Content = content,
            CitedSourcesJson = JsonSerializer.Serialize(citedSources),
        };
    }

    public IReadOnlyList<string> CitedSources() =>
        JsonSerializer.Deserialize<List<string>>(CitedSourcesJson) ?? [];
}
