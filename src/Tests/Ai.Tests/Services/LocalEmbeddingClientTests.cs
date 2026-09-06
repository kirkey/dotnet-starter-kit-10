using FSH.Modules.Ai.Services;
using Shouldly;

namespace Ai.Tests.Services;

public class LocalEmbeddingClientTests
{
    private readonly LocalEmbeddingClient _sut = new();

    #region Happy Path

    [Fact]
    public async Task EmbedAsync_Should_ReturnNormalized3072Vector()
    {
        var vector = await _sut.EmbedAsync("the quick brown fox");

        vector.Length.ShouldBe(3072);
        MathF.Sqrt(vector.Sum(v => v * v)).ShouldBe(1f, 0.0001);
    }

    [Fact]
    public async Task EmbedAsync_Should_BeDeterministic()
    {
        var first = await _sut.EmbedAsync("deterministic text");
        var second = await _sut.EmbedAsync("deterministic text");

        first.ShouldBe(second);
    }

    [Fact]
    public async Task EmbedAsync_Should_RankRelatedAboveUnrelated()
    {
        static double Cosine(float[] a, float[] b) => a.Zip(b).Sum(p => p.First * p.Second);

        var query = await _sut.EmbedAsync("postgres vector database");
        var related = await _sut.EmbedAsync("postgres pgvector similarity search database");
        var unrelated = await _sut.EmbedAsync("chocolate cake recipe baking");

        Cosine(query, related).ShouldBeGreaterThan(Cosine(query, unrelated));
    }

    #endregion

    #region Edge Cases

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task EmbedAsync_Should_RejectEmptyText(string text)
    {
        await Should.ThrowAsync<ArgumentException>(() => _sut.EmbedAsync(text));
    }

    #endregion
}
