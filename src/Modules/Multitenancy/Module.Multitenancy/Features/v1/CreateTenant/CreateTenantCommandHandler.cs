using FSH.Module.Multitenancy.Contracts;
using FSH.Module.Multitenancy.Contracts.v1.CreateTenant;
using FSH.Module.Multitenancy.Provisioning;
using Mediator;

namespace FSH.Module.Multitenancy.Features.v1.CreateTenant;

public class CreateTenantCommandHandler(ITenantService tenantService, ITenantProvisioningService provisioningService)
    : ICommandHandler<CreateTenantCommand, CreateTenantCommandResponse>
{
    public async ValueTask<CreateTenantCommandResponse> Handle(CreateTenantCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        string tenantId = await tenantService.CreateAsync(
            command.Id,
            command.Name,
            command.ConnectionString,
            command.AdminEmail,
            command.Issuer,
            cancellationToken);

        Provisioning.TenantProvisioning provisioning = await provisioningService.StartAsync(tenantId, cancellationToken);

        return new CreateTenantCommandResponse(
            tenantId,
            provisioning.CorrelationId,
            provisioning.Status.ToString());
    }
}
