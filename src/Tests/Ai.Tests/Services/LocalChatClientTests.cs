using FSH.Modules.Ai.Services;
using Shouldly;

namespace Ai.Tests.Services;

public class LocalChatClientTests
{
    private readonly LocalChatClient _sut = new();

    #region Happy Path

    [Fact]
    public async Task CompleteAsync_Should_AnswerFromChunks_With_Citations()
    {
        var chunks = new List<RetrievedChunk>
        {
            new(Guid.NewGuid(), "Guide", "Postgres pgvector stores embeddings for similarity search over documents.", 0.9),
        };

        var answer = await _sut.CompleteAsync(
            [new ChatTurn("user", "Where are embeddings stored?")],
            chunks,
            "Default");

        answer.CitedSources.ShouldContain("Guide");
        answer.UsedModel.ShouldBe(LocalChatClient.LocalModelName);
    }

    [Fact]
    public async Task CompleteAsync_Should_FallBackHonestly_When_NoChunks()
    {
        var answer = await _sut.CompleteAsync(
            [new ChatTurn("user", "Anything at all?")],
            [],
            "Default");

        answer.Text.ShouldContain("couldn't find anything");
        answer.CitedSources.ShouldBeEmpty();
    }

    [Fact]
    public async Task CompleteAsync_Should_FallBackHonestly_When_ChunksShareNoTerms()
    {
        var chunks = new List<RetrievedChunk>
        {
            new(Guid.NewGuid(), "Cookbook", "Whisk the batter gently until golden brown and fragrant.", 0.9),
        };

        var answer = await _sut.CompleteAsync(
            [new ChatTurn("user", "Where are embeddings stored?")],
            chunks,
            "Default");

        answer.Text.ShouldContain("couldn't find anything");
        answer.CitedSources.ShouldBeEmpty();
    }

    #endregion
}
