using FSH.Module.Multitenancy.Contracts.Dtos;
using Mediator;

namespace FSH.Module.Multitenancy.Contracts.v1.TenantProvisioning;

public sealed record RetryTenantProvisioningCommand(string TenantId) : ICommand<TenantProvisioningStatusDto>;
