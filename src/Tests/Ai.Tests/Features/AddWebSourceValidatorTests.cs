using FSH.Modules.Ai.Contracts.v1.Sources;
using FSH.Modules.Ai.Features.v1.Sources.AddWebSource;
using Shouldly;

namespace Ai.Tests.Features;

public class AddWebSourceCommandValidatorTests
{
    private readonly AddWebSourceCommandValidator _sut = new();

    #region Happy Path

    [Fact]
    public void Validate_Should_Pass_For_HttpsUrl()
    {
        _sut.Validate(new AddWebSourceCommand("https://example.com/article", null)).IsValid.ShouldBeTrue();
    }

    #endregion

    #region Edge Cases

    [Theory]
    [InlineData("not-a-url")]
    [InlineData("ftp://files.test/x")]
    [InlineData("")]
    public void Validate_Should_Fail_For_NonHttpUrl(string url)
    {
        _sut.Validate(new AddWebSourceCommand(url, null)).IsValid.ShouldBeFalse();
    }

    [Fact]
    public void Validate_Should_Fail_For_LongName()
    {
        _sut.Validate(new AddWebSourceCommand("https://example.com", new string('n', 201))).IsValid.ShouldBeFalse();
    }

    #endregion
}
