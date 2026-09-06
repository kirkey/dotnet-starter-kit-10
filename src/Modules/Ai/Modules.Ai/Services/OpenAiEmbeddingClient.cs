using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace FSH.Modules.Ai.Services;

/// <summary>Embeddings against any OpenAI-compatible `/embeddings` endpoint.</summary>
public sealed class OpenAiEmbeddingClient(IHttpClientFactory httpFactory, string baseUrl, string model, string apiKey)
    : IEmbeddingClient
{
    public string ModelName => model;
    public int Dimensions => 0; // provider-declared; validated against the store width at attach time

    public async Task<float[]> EmbedAsync(string text, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        var client = httpFactory.CreateClient("AiProviders");
        using var request = new HttpRequestMessage(
            HttpMethod.Post, $"{baseUrl.TrimEnd('/')}/embeddings")
        {
            Content = JsonContent.Create(new EmbeddingRequest(model, text))
        };
        if (!string.IsNullOrWhiteSpace(apiKey))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
        }

        using var response = await client.SendAsync(request, ct).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<EmbeddingResponse>(ct).ConfigureAwait(false);
        var vector = payload?.Data is { Count: > 0 } data
            ? data[0].Embedding
            : throw new InvalidOperationException($"Embedding endpoint '{baseUrl}' returned no vector.");
        return vector;
    }

    private sealed record EmbeddingRequest(
        [property: JsonPropertyName("model")] string Model,
        [property: JsonPropertyName("input")] string Input);

    private sealed record EmbeddingResponse(
        [property: JsonPropertyName("data")] IReadOnlyList<EmbeddingDatum> Data);

    private sealed record EmbeddingDatum(
        [property: JsonPropertyName("embedding")] float[] Embedding);
}
