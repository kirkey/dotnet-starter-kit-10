using FSH.Framework.Core.Domain;
using Pgvector;

namespace FSH.Modules.Ai.Domain;

/// <summary>
/// One retrievable chunk of a source's Markdown twin. Created via the owning
/// <see cref="AiSource"/> aggregate so chunks are never persisted independently.
/// The embedding is null until the embed step runs. Dimensions are fixed at 3072
/// with cosine distance: half precision keeps HNSW usable past its 2000-dimension
/// float limit, and zero-padding preserves cosine ordering for unit-norm embeddings.
/// </summary>
public sealed class AiChunk : BaseEntity<Guid>
{
    public const int EmbeddingDimensions = 3072;

    public Guid SourceId { get; private set; }
    public int Ordinal { get; private set; }
    public string Content { get; private set; } = default!;
    public int CharCount { get; private set; }
    public HalfVector? Embedding { get; private set; }
    public string? EmbeddingModel { get; private set; }

    private AiChunk() { }

    internal static AiChunk Create(Guid sourceId, int ordinal, string content)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        return new AiChunk
        {
            Id = Guid.CreateVersion7(),
            SourceId = sourceId,
            Ordinal = ordinal,
            Content = content,
            CharCount = content.Length,
        };
    }

    public void AttachEmbedding(float[] vector, string model)
    {
        ArgumentNullException.ThrowIfNull(vector);
        ArgumentException.ThrowIfNullOrWhiteSpace(model);

        if (vector.Length > EmbeddingDimensions)
        {
            throw new ArgumentOutOfRangeException(
                nameof(vector),
                $"Embedding has {vector.Length} dimensions but the store supports at most {EmbeddingDimensions}.");
        }

        if (vector.Length == EmbeddingDimensions)
        {
            Embedding = new HalfVector(Array.ConvertAll(vector, v => (Half)v));
        }
        else
        {
            // Zero-pad smaller provider vectors up to the store width. Padding with
            // zeros preserves cosine ordering for unit-norm embeddings.
            var padded = new Half[EmbeddingDimensions];
            Array.ConvertAll(vector, v => (Half)v).CopyTo(padded, 0);
            Embedding = new HalfVector(padded);
        }

        EmbeddingModel = model;
    }
}
