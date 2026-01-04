using FSH.Module.Multitenancy.Contracts;
using FSH.Module.Multitenancy.Contracts.Dtos;
using FSH.Module.Multitenancy.Contracts.v1.ChangeTenantActivation;
using Mediator;

namespace FSH.Module.Multitenancy.Features.v1.ChangeTenantActivation;

public sealed class ChangeTenantActivationCommandHandler(ITenantService tenantService)
    : ICommandHandler<ChangeTenantActivationCommand, TenantLifecycleResultDto>
{
    public async ValueTask<TenantLifecycleResultDto> Handle(ChangeTenantActivationCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        string message;

        if (command.IsActive)
        {
            message = await tenantService.ActivateAsync(command.TenantId, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            message = await tenantService.DeactivateAsync(command.TenantId).ConfigureAwait(false);
        }

        TenantStatusDto status = await tenantService.GetStatusAsync(command.TenantId).ConfigureAwait(false);

        return new TenantLifecycleResultDto
        {
            TenantId = status.Id,
            IsActive = status.IsActive,
            ValidUpto = status.ValidUpto,
            Message = message
        };
    }
}
