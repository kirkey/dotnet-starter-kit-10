using FluentValidation;
using FSH.Modules.Multitenancy.Contracts.v1.TenantProvisioning;

namespace FSH.Modules.Multitenancy.Features.v1.TenantProvisioning.RetryTenantProvisioning;

public sealed class RetryTenantProvisioningCommandValidator : AbstractValidator<RetryTenantProvisioningCommand>
{
    public RetryTenantProvisioningCommandValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
    }
}
