using FSH.Modules.Identity.Features.v1.TwoFactor.Enroll;
using FSH.Modules.Identity.Contracts.v1.TwoFactor;
using Shouldly;
using Xunit;

namespace Identity.Tests.Validators;

public sealed class EnrollTwoFactorCommandValidatorTests
{
    [Fact]
    public void Accepts_parameterless_command()
        => new EnrollTwoFactorCommandValidator().Validate(new EnrollTwoFactorCommand()).IsValid.ShouldBeTrue();
}
