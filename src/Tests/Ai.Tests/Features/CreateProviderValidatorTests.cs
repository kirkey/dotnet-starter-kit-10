using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Providers;
using FSH.Modules.Ai.Features.v1.Providers.CreateProvider;
using Shouldly;

namespace Ai.Tests.Features;

public class CreateProviderValidatorTests
{
    private readonly CreateProviderCommandValidator _sut = new();

    #region Happy Path

    [Fact]
    public void Validate_Should_Pass_For_LocalProvider()
    {
        var command = new CreateProviderCommand(
            "Local", AiProviderType.Local, null, "local-extractive", "local-hash-3072", 3072, []);

        _sut.Validate(command).IsValid.ShouldBeTrue();
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Validate_Should_Fail_When_RemoteHasNoEndpoint()
    {
        var command = new CreateProviderCommand(
            "Remote", AiProviderType.OpenAiCompatible, null, "gpt-x", null, 3072, []);

        _sut.Validate(command).IsValid.ShouldBeFalse();
    }

    [Fact]
    public void Validate_Should_Fail_When_NoModelDeclared()
    {
        var command = new CreateProviderCommand(
            "Empty", AiProviderType.Local, null, null, null, 3072, []);

        _sut.Validate(command).IsValid.ShouldBeFalse();
    }

    #endregion
}
