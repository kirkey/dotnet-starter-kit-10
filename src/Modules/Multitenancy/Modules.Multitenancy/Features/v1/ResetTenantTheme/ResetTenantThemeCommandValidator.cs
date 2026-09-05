using FluentValidation;
using FSH.Modules.Multitenancy.Contracts.v1.ResetTenantTheme;

namespace FSH.Modules.Multitenancy.Features.v1.ResetTenantTheme;

public sealed class ResetTenantThemeCommandValidator : AbstractValidator<ResetTenantThemeCommand>
{
    public ResetTenantThemeCommandValidator()
    {
        // Parameterless command (operates on the current tenant): no rules to check,
        // but the pairing contract requires a validator to exist.
    }
}
