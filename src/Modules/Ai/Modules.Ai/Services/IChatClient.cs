namespace FSH.Modules.Ai.Services;

public sealed record ChatTurn(string Role, string Content);

public sealed record RetrievedChunk(Guid ChunkId, string SourceName, string Content, double Score);

public sealed record ChatAnswer(string Text, IReadOnlyList<string> CitedSources, string UsedModel, string UsedVariant);

public interface IChatClient
{
    string ModelName { get; }

    Task<ChatAnswer> CompleteAsync(
        IReadOnlyList<ChatTurn> turns,
        IReadOnlyList<RetrievedChunk> chunks,
        string variant,
        bool allowUngrounded = false,
        CancellationToken ct = default);
}
