using FluentValidation;
using FSH.Modules.Identity.Contracts.v1.Impersonation.EndImpersonation;

namespace FSH.Modules.Identity.Features.v1.Impersonation.EndImpersonation;

public sealed class EndImpersonationCommandValidator : AbstractValidator<EndImpersonationCommand>
{
    public EndImpersonationCommandValidator()
    {
        // Parameterless command (operates on the current impersonation session):
        // no rules to check, but the pairing contract requires a validator to exist.
    }
}
