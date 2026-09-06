using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pgvector.EntityFrameworkCore;

namespace FSH.Modules.Ai.Services;

public sealed record ChatRunResult(AiChatMessageDto UserMessage, AiChatMessageDto AssistantMessage);

public interface IChatRunner
{
    Task<ChatRunResult> RunAsync(
        Guid sessionId,
        Guid ownerId,
        string content,
        Func<string, Task>? onSegment,
        CancellationToken ct = default);
}

/// <summary>
/// RAG turn orchestration: resolve the chat client (fail fast, before persisting), retrieve the
/// top tenant-scoped chunks, complete, persist both messages, and fan answer segments out.
/// </summary>
public sealed class ChatRunner(
    AiDbContext db,
    IAiProviderSelector selector,
    ILogger<ChatRunner> logger)
    : IChatRunner
{
    private const int TopK = 5;
    private const int HistoryTurns = 10;
    private const int SegmentChars = 200;

    public async Task<ChatRunResult> RunAsync(
        Guid sessionId,
        Guid ownerId,
        string content,
        Func<string, Task>? onSegment,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        var session = await db.ChatSessions
            .Include(s => s.Messages)
            .FirstOrDefaultAsync(s => s.Id == sessionId && s.OwnerId == ownerId, ct)
            .ConfigureAwait(false)
            ?? throw new FSH.Framework.Core.Exceptions.NotFoundException($"Chat session {sessionId} was not found.");

        if (session.AgentId.HasValue)
        {
            await GuardAgentAccessAsync(db, session.AgentId.Value, ownerId, ct).ConfigureAwait(false);
        }

        // Fail fast on misconfiguration before persisting anything.
        var chat = await selector.SelectChatClientAsync(session.Model, session.AgentId, ct).ConfigureAwait(false);

        var userMessage = session.AddMessage("user", content.Trim(), []);
        // Explicit state: adding to the collection of an already-dirtied parent (MessageCount/
        // LastActivity change above) mis-tracks the new row as Modified through DetectChanges,
        // which fails as a 0-row concurrency violation. Fresh rows are always inserts.
        db.Entry(userMessage).State = EntityState.Added;
        if (session.MessageCount == 1 && session.Title == "New chat")
        {
            session.Rename(content.Trim().Length <= 60 ? content.Trim() : content.Trim()[..60]);
        }

        await db.SaveChangesAsync(ct).ConfigureAwait(false);

        var chunks = await RetrieveAsync(content, ct).ConfigureAwait(false);

        var turns = session.Messages
            .OrderBy(m => m.Ordinal)
            .TakeLast(HistoryTurns)
            .Select(m => new ChatTurn(m.Role, m.Content))
            .ToList();

        var answer = await chat.CompleteAsync(turns, chunks, session.Variant.ToString(), allowUngrounded: false, ct).ConfigureAwait(false);

        var assistantMessage = session.AddMessage("assistant", answer.Text, answer.CitedSources);
        db.Entry(assistantMessage).State = EntityState.Added;
        await db.SaveChangesAsync(ct).ConfigureAwait(false);

        if (onSegment is not null)
        {
            foreach (var segment in Segment(answer.Text))
            {
                await onSegment(segment).ConfigureAwait(false);
            }
        }

        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation(
                "[Ai] chat turn in session {SessionId}: {Chars} chars citing {Cited}",
                sessionId, answer.Text.Length, answer.CitedSources.Count);
        }

        return new ChatRunResult(ToDto(userMessage), ToDto(assistantMessage));
    }

    internal async Task<IReadOnlyList<RetrievedChunk>> RetrieveAsync(string question, CancellationToken ct)
    {
        var embedder = await selector.SelectEmbeddingClientAsync(ct).ConfigureAwait(false);
        var vector = await embedder.EmbedAsync(question, ct).ConfigureAwait(false);
        var query = AiVector.ToStoreVector(vector);

        var hits = await db.Chunks
            .Where(c => c.Embedding != null)
            .Join(
                db.Sources.Where(s => s.Status == AiSourceStatus.Ready),
                c => c.SourceId,
                s => s.Id,
                (c, s) => new { Chunk = c, SourceName = s.Name, Distance = c.Embedding!.CosineDistance(query) })
            .OrderBy(x => x.Distance)
            .Take(TopK)
            .ToListAsync(ct)
            .ConfigureAwait(false);

        return hits
            .Select(x => new RetrievedChunk(x.Chunk.Id, x.SourceName, x.Chunk.Content, 1 - x.Distance))
            .ToList();
    }

    internal static IEnumerable<string> Segment(string text)
    {
        for (var start = 0; start < text.Length; start += SegmentChars)
        {
            yield return text.Substring(start, Math.Min(SegmentChars, text.Length - start));
        }
    }

    private static AiChatMessageDto ToDto(Domain.AiChatMessage message) =>
        new(message.Id, message.Role, message.Content, message.CitedSources());

    internal static async Task GuardAgentAccessAsync(AiDbContext db, Guid agentId, Guid userId, CancellationToken ct)
    {
        var agent = await db.Agents
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == agentId, ct)
            .ConfigureAwait(false);
        if (agent is not null
            && agent.AccessMode == Contracts.Dtos.AgentAccessMode.Selected
            && !agent.AccessUserIds().Contains(userId.ToString(), StringComparer.OrdinalIgnoreCase))
        {
            throw new FSH.Framework.Core.Exceptions.ForbiddenException();
        }
    }
}
