using FSH.Module.Multitenancy.Contracts.Dtos;
using Mediator;

namespace FSH.Module.Multitenancy.Contracts.v1.TenantProvisioning;

public sealed record GetTenantProvisioningStatusQuery(string TenantId) : IQuery<TenantProvisioningStatusDto>;
