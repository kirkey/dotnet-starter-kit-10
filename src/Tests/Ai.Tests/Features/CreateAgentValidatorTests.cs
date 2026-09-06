using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Agents;
using FSH.Modules.Ai.Features.v1.Agents.CreateAgent;
using Shouldly;

namespace Ai.Tests.Features;

public class CreateAgentValidatorTests
{
    private readonly CreateAgentCommandValidator _sut = new();

    private static CreateAgentCommand Valid(AiVariant variant = AiVariant.High) =>
        new(
            Guid.NewGuid(),
            "Helper",
            "Help with tickets.",
            ["tickets"],
            "codex",
            "gpt-x",
            variant,
            AgentAccessMode.Department,
            []);

    #region Happy Path

    [Fact]
    public void Validate_Should_Pass_For_SupportedVariant()
    {
        _sut.Validate(Valid(AiVariant.High)).IsValid.ShouldBeTrue();
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Validate_Should_Fail_When_SelectedAccessHasNoUsers()
    {
        var command = Valid() with { AccessMode = AgentAccessMode.Selected };

        _sut.Validate(command).IsValid.ShouldBeFalse();
    }

    [Fact]
    public void Validate_Should_Fail_When_InstructionsMissing()
    {
        var command = Valid() with { Instructions = "" };

        _sut.Validate(command).IsValid.ShouldBeFalse();
    }

    #endregion
}
