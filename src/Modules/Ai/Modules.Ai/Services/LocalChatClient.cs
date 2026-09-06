namespace FSH.Modules.Ai.Services;

/// <summary>
/// Built-in deterministic chat: extractive answers over the retrieved chunks with an honest
/// fallback when nothing covers the question. Offline-capable; production tenants point the
/// chat default at an OpenAI-compatible provider instead.
/// </summary>
public sealed class LocalChatClient : IChatClient
{
    public const string LocalModelName = "local-extractive";

    public string ModelName => LocalModelName;

    public Task<ChatAnswer> CompleteAsync(
        IReadOnlyList<ChatTurn> turns,
        IReadOnlyList<RetrievedChunk> chunks,
        string variant,
        bool allowUngrounded = false,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(turns);
        ArgumentNullException.ThrowIfNull(chunks);
        if (turns.Count == 0)
        {
            throw new ArgumentException("At least one turn is required.", nameof(turns));
        }

        var question = turns[turns.Count - 1].Content;
        if (chunks.Count == 0 && !allowUngrounded)
        {
            return Task.FromResult(Uncovered(variant));
        }

        if (chunks.Count == 0)
        {
            // Agent-run mode: deterministic echo summary (no provider call, no hallucination).
            return Task.FromResult(new ChatAnswer(
                $"Run input received ({question.Length} chars). Configure a chat provider on this tenant for generated output.",
                [],
                LocalModelName,
                variant));
        }

        var terms = LocalEmbeddingClient.Tokenize(question).ToHashSet(StringComparer.Ordinal);
        var sentences = chunks
            .SelectMany((c, i) => SplitSentences(c.Content).Select(s => (Sentence: s, Source: c.SourceName, Index: i)))
            .Select(x => (x.Sentence, x.Source, x.Index, Score: x.Sentence.Split(' ').Count(w => terms.Contains(w.Trim(' ', '.', ',', ';', ':', '!', '?', '"', '\'').ToUpperInvariant()))))
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.Index)
            .Take(4)
            .ToList();

        if (sentences.Count == 0)
        {
            // Sources exist but share no terms with the question: say so plainly, no citation.
            return Task.FromResult(Uncovered(variant));
        }

        var cited = sentences.Select(s => s.Source).Distinct().ToList();
        var lines = sentences.Select(s => $"- {s.Sentence.Trim()} [{s.Source}]");
        var text = $"Based on your sources:\n\n{string.Join("\n", lines)}\n\nSources: {string.Join(", ", cited)}";
        return Task.FromResult(new ChatAnswer(text, cited, LocalModelName, variant));
    }

    private static ChatAnswer Uncovered(string variant) =>
        new("I couldn't find anything in your knowledge sources about that. Add a source that covers it and ask again.",
            [],
            LocalModelName,
            variant);

    internal static IReadOnlyList<string> SplitSentences(string text) =>
        text.Split(['.', '!', '?', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(s => s.Length > 20)
            .ToList();
}
