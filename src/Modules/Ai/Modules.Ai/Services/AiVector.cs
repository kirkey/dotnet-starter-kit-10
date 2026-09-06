using FSH.Modules.Ai.Domain;
using Pgvector;

namespace FSH.Modules.Ai.Services;

/// <summary>Converts provider embedding vectors to store width (zero-padded like AttachEmbedding).</summary>
public static class AiVector
{
    public static HalfVector ToStoreVector(float[] vector)
    {
        ArgumentNullException.ThrowIfNull(vector);
        if (vector.Length > AiChunk.EmbeddingDimensions)
        {
            throw new ArgumentOutOfRangeException(
                nameof(vector),
                $"Embedding has {vector.Length} dimensions but the store supports at most {AiChunk.EmbeddingDimensions}.");
        }

        if (vector.Length == AiChunk.EmbeddingDimensions)
        {
            return new HalfVector(Array.ConvertAll(vector, v => (Half)v));
        }

        var padded = new Half[AiChunk.EmbeddingDimensions];
        Array.ConvertAll(vector, v => (Half)v).CopyTo(padded, 0);
        return new HalfVector(padded);
    }
}
