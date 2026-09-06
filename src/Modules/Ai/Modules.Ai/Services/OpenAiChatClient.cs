using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace FSH.Modules.Ai.Services;

/// <summary>Chat completions against any OpenAI-compatible `/chat/completions` endpoint.</summary>
public sealed class OpenAiChatClient(IHttpClientFactory httpFactory, string baseUrl, string model, string apiKey)
    : IChatClient
{
    public string ModelName => model;

    public async Task<ChatAnswer> CompleteAsync(
        IReadOnlyList<ChatTurn> turns,
        IReadOnlyList<RetrievedChunk> chunks,
        string variant,
        bool allowUngrounded = false,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(turns);
        ArgumentNullException.ThrowIfNull(chunks);

        var client = httpFactory.CreateClient("AiProviders");
        List<ChatApiMessage> messages;
        if (chunks.Count == 0 && !allowUngrounded)
        {
            messages = BuildUngroundedMessages(turns);
        }
        else if (chunks.Count == 0)
        {
            messages = turns.Select(t => new ChatApiMessage(t.Role, t.Content)).ToList();
        }
        else
        {
            messages = BuildGroundedMessages(turns, chunks);
        }

        using var request = new HttpRequestMessage(
            HttpMethod.Post, $"{baseUrl.TrimEnd('/')}/chat/completions")
        {
            Content = JsonContent.Create(new ChatRequest(model, messages))
        };
        if (!string.IsNullOrWhiteSpace(apiKey))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
        }

        using var response = await client.SendAsync(request, ct).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<ChatResponse>(ct).ConfigureAwait(false);
        var text = payload?.Choices is { Count: > 0 } choices
            ? choices[0].Message.Content?.Trim()
            : null;
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new InvalidOperationException($"Chat endpoint '{baseUrl}' returned no message.");
        }

        return new ChatAnswer(text, chunks.Select(c => c.SourceName).Distinct().ToList(), model, variant);
    }

    internal static List<ChatApiMessage> BuildUngroundedMessages(IReadOnlyList<ChatTurn> turns) =>
        [
            // No covering sources: constrain the provider to honesty instead of parametric recall.
            new("system", "You have no knowledge sources covering this conversation. Say plainly that you cannot answer from the user's sources instead of inventing an answer."),
            .. turns.Select(t => new ChatApiMessage(t.Role, t.Content)),
        ];

    internal static List<ChatApiMessage> BuildGroundedMessages(
        IReadOnlyList<ChatTurn> turns, IReadOnlyList<RetrievedChunk> chunks)
    {
        if (chunks.Count == 0)
        {
            return BuildUngroundedMessages(turns);
        }

        var context = string.Join("\n\n---\n\n", chunks.Select((c, i) => $"[Source {i + 1}: {c.SourceName}]\n{c.Content}"));
        return
        [
            new("system", $"Answer using only the sources below. Cite them as [Source N]. If they do not cover the question, say so plainly.\n\n{context}"),
            .. turns.Select(t => new ChatApiMessage(t.Role, t.Content)),
        ];
    }

    internal sealed record ChatApiMessage(
        [property: JsonPropertyName("role")] string Role,
        [property: JsonPropertyName("content")] string Content);

    private sealed record ChatRequest(
        [property: JsonPropertyName("model")] string Model,
        [property: JsonPropertyName("messages")] IReadOnlyList<ChatApiMessage> Messages);

    private sealed record ChatResponse(
        [property: JsonPropertyName("choices")] IReadOnlyList<ChatChoice> Choices);

    private sealed record ChatChoice(
        [property: JsonPropertyName("message")] ChatApiMessage Message);
}
