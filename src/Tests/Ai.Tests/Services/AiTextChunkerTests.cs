using FSH.Modules.Ai.Services;
using Shouldly;

namespace Ai.Tests.Services;

public class AiTextChunkerTests
{
    private readonly AiTextChunker _sut = new();

    #region Happy Path

    [Fact]
    public void Chunk_Should_ReturnSingleChunk_For_ShortText()
    {
        var chunks = _sut.Chunk("Short paragraph one.\n\nShort paragraph two.");

        chunks.ShouldHaveSingleItem();
    }

    [Fact]
    public void Chunk_Should_SplitLongText_IntoBoundedChunks()
    {
        var paragraph = new string('w', 500);
        var text = string.Join("\n\n", Enumerable.Repeat(paragraph, 10));

        var chunks = _sut.Chunk(text);

        chunks.Count.ShouldBeGreaterThan(1);
        chunks.ShouldAllBe(c => c.Length <= AiTextChunker.TargetChars);
    }

    [Fact]
    public void Chunk_Should_HardSplit_OversizedParagraph()
    {
        var chunks = _sut.Chunk(new string('v', 5000));

        chunks.Count.ShouldBeGreaterThan(1);
        string.Concat(chunks).Length.ShouldBeGreaterThanOrEqualTo(5000);
    }

    #endregion

    #region Edge Cases

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Chunk_Should_RejectEmptyMarkdown(string markdown)
    {
        Should.Throw<ArgumentException>(() => _sut.Chunk(markdown));
    }

    #endregion
}
