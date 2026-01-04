using FSH.Framework.Shared.Persistence;
using FSH.Module.Multitenancy.Contracts;
using FSH.Module.Multitenancy.Contracts.Dtos;
using FSH.Module.Multitenancy.Contracts.v1.GetTenants;
using Mediator;

namespace FSH.Module.Multitenancy.Features.v1.GetTenants;

public sealed class GetTenantsQueryHandler(ITenantService tenantService)
    : IQueryHandler<GetTenantsQuery, PagedResponse<TenantDto>>
{
    public async ValueTask<PagedResponse<TenantDto>> Handle(GetTenantsQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await tenantService.GetAllAsync(query, cancellationToken).ConfigureAwait(false);
    }
}
