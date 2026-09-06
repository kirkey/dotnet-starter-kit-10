using System.Text.RegularExpressions;
using FSH.Modules.Ai.Domain;

namespace FSH.Modules.Ai.Services;

/// <summary>
/// Built-in deterministic embedding: hashed token-frequency vector, L2-normalized. Zero
/// infrastructure, offline-capable, and cosine-meaningful for retrieval over small corpora.
/// Production tenants point the embedding default at an OpenAI-compatible provider instead.
/// </summary>
public sealed partial class LocalEmbeddingClient : IEmbeddingClient
{
    public const string LocalModelName = "local-hash-3072";

    public string ModelName => LocalModelName;
    public int Dimensions => AiChunk.EmbeddingDimensions;

    public Task<float[]> EmbedAsync(string text, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        var vector = new float[Dimensions];
        foreach (var token in Tokenize(text))
        {
            var hash = StableHash(token);
            vector[(int)(hash % (uint)Dimensions)] += 1f;
            vector[(int)((hash >> 16) % (uint)Dimensions)] += 0.5f;
        }

        var norm = MathF.Sqrt(vector.Sum(v => v * v));
        if (norm > 0)
        {
            for (var i = 0; i < vector.Length; i++)
            {
                vector[i] /= norm;
            }
        }

        return Task.FromResult(vector);
    }

    internal static IEnumerable<string> Tokenize(string text) =>
        TokenPattern().Matches(text.ToUpperInvariant()).Select(m => m.Value);

    private static uint StableHash(string token)
    {
        // FNV-1a 32-bit: stable across processes/runs, unlike string.GetHashCode.
        var hash = 2166136261u;
        foreach (var c in token)
        {
            hash ^= c;
            hash *= 16777619u;
        }

        return hash;
    }

    [GeneratedRegex(@"[A-Z0-9]{2,}")]
    private static partial Regex TokenPattern();
}
