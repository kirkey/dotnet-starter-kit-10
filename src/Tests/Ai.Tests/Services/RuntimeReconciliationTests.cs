using FSH.Modules.Ai.Services;
using Shouldly;

namespace Ai.Tests.Services;

public class RuntimeReconciliationTests
{
    #region Happy Path

    [Fact]
    public void Compute_Should_MarkSeenOnline_And_MissingOffline()
    {
        var rows = RuntimeReconciliation.Compute(
            ["codex", "claude-code"],
            new Dictionary<string, (string?, bool)>(StringComparer.OrdinalIgnoreCase),
            [new DetectedAgent("codex", "OpenAI Codex", "1.2.3", true, "/home/x/.codex")]);

        var codex = rows.First(r => r.Family == "codex");
        codex.IsOnline.ShouldBeTrue();
        codex.DetectedVersion.ShouldBe("1.2.3");
        codex.IsNew.ShouldBeTrue();

        var claude = rows.First(r => r.Family == "claude-code");
        claude.IsOnline.ShouldBeFalse();
        claude.IsNew.ShouldBeTrue();
    }

    [Fact]
    public void Compute_Should_FlipPreviouslySeen_To_Offline()
    {
        var rows = RuntimeReconciliation.Compute(
            ["codex"],
            new Dictionary<string, (string?, bool)>(StringComparer.OrdinalIgnoreCase)
            {
                ["codex"] = ("1.2.3", true),
            },
            []);

        var codex = rows.ShouldHaveSingleItem();
        codex.IsOnline.ShouldBeFalse();
        codex.IsNew.ShouldBeFalse();
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Compute_Should_IncludeUnknownDetectedFamilies()
    {
        var rows = RuntimeReconciliation.Compute(
            ["codex"],
            new Dictionary<string, (string?, bool)>(StringComparer.OrdinalIgnoreCase),
            [new DetectedAgent("future-cli", "Future CLI", null, false, "/home/x/.future")]);

        rows.Select(r => r.Family).ShouldContain("future-cli");
    }

    #endregion
}
