using FluentValidation;
using FSH.Modules.Identity.Contracts.v1.TwoFactor;

namespace FSH.Modules.Identity.Features.v1.TwoFactor.Enroll;

public sealed class EnrollTwoFactorCommandValidator : AbstractValidator<EnrollTwoFactorCommand>
{
    public EnrollTwoFactorCommandValidator()
    {
        // Parameterless command (operates on the current user): no rules to check,
        // but the pairing contract requires a validator to exist.
    }
}
