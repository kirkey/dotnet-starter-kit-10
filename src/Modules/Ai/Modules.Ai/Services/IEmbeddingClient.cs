namespace FSH.Modules.Ai.Services;

public interface IEmbeddingClient
{
    string ModelName { get; }
    int Dimensions { get; }
    Task<float[]> EmbedAsync(string text, CancellationToken ct = default);
}
