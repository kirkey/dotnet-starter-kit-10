using FSH.Module.Multitenancy.Contracts;
using FSH.Module.Multitenancy.Contracts.Dtos;
using FSH.Module.Multitenancy.Contracts.v1.GetTenantStatus;
using Mediator;

namespace FSH.Module.Multitenancy.Features.v1.GetTenantStatus;

public sealed class GetTenantStatusQueryHandler(ITenantService tenantService)
    : IQueryHandler<GetTenantStatusQuery, TenantStatusDto>
{
    public async ValueTask<TenantStatusDto> Handle(GetTenantStatusQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);
        return await tenantService.GetStatusAsync(query.TenantId);
    }
}

