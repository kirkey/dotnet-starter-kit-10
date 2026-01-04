using FluentValidation;
using FSH.Module.Multitenancy.Contracts.v1.ChangeTenantActivation;

namespace FSH.Module.Multitenancy.Features.v1.ChangeTenantActivation;

internal sealed class ChangeTenantActivationCommandValidator : AbstractValidator<ChangeTenantActivationCommand>
{
    public ChangeTenantActivationCommandValidator() =>
       RuleFor(t => t.TenantId)
           .NotEmpty();
}