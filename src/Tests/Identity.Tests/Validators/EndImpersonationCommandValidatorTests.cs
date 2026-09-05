using FSH.Modules.Identity.Features.v1.Impersonation.EndImpersonation;
using FSH.Modules.Identity.Contracts.v1.Impersonation.EndImpersonation;
using Shouldly;
using Xunit;

namespace Identity.Tests.Validators;

public sealed class EndImpersonationCommandValidatorTests
{
    [Fact]
    public void Accepts_parameterless_command()
        => new EndImpersonationCommandValidator().Validate(new EndImpersonationCommand()).IsValid.ShouldBeTrue();
}
