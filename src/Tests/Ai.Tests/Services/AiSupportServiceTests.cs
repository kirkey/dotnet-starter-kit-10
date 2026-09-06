using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Services;
using Shouldly;

namespace Ai.Tests.Services;

public class RuntimeVariantMatrixTests
{
    #region Happy Path

    [Fact]
    public void SupportedTiers_Should_ReturnAllFour_For_ClaudeCode()
    {
        RuntimeVariantMatrix.SupportedTiers("claude-code").ShouldBe(
            [AiVariant.Default, AiVariant.Normal, AiVariant.High, AiVariant.ExtraHigh]);
    }

    [Fact]
    public void IsSupported_Should_RejectExtraHigh_For_Codex()
    {
        RuntimeVariantMatrix.IsSupported("codex", AiVariant.ExtraHigh).ShouldBeFalse();
        RuntimeVariantMatrix.IsSupported("codex", AiVariant.High).ShouldBeTrue();
    }

    #endregion

    #region Edge Cases

    [Theory]
    [InlineData("unknown-cli")]
    [InlineData("")]
    public void SupportedTiers_Should_FallBackToDefault_For_UnknownFamily(string family)
    {
        RuntimeVariantMatrix.SupportedTiers(family).ShouldBe([AiVariant.Default]);
    }

    #endregion
}

public class LocalAgentDetectorTests
{
    #region Happy Path

    [Fact]
    public void Detect_Should_FindAgentDir_With_Credentials()
    {
        var home = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(home, ".claude"));
        File.WriteAllText(Path.Combine(home, ".claude", ".credentials.json"), "{}");
        try
        {
            var detector = new LocalAgentDetector(home);

            var found = detector.Detect();

            var entry = found.ShouldHaveSingleItem();
            entry.Family.ShouldBe("claude-code");
            entry.HasCredentials.ShouldBeTrue();
        }
        finally
        {
            Directory.Delete(home, recursive: true);
        }
    }

    [Fact]
    public void Detect_Should_ReturnEmpty_When_NoAgents()
    {
        var home = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(home);
        try
        {
            new LocalAgentDetector(home).Detect().ShouldBeEmpty();
        }
        finally
        {
            Directory.Delete(home, recursive: true);
        }
    }

    #endregion
}
