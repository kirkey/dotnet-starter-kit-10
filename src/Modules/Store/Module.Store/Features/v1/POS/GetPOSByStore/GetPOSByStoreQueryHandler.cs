using FSH.Framework.Core.Context;
using FSH.Module.Store.Contracts.v1.POS;
using FSH.Module.Store.Data;
using Mediator;

namespace FSH.Module.Store.Features.v1.POS.GetPOSByStore;

public sealed class GetPOSByStoreQueryHandler(
    StoreDbContext context,
    ICurrentUser currentUser)
    : IQueryHandler<GetPOSByStoreQuery, List<POSResponse>>
{
    public async ValueTask<List<POSResponse>> Handle(
        GetPOSByStoreQuery query,
        CancellationToken cancellationToken)
    {
        var posTerminals = await context.POSTerminals
            .AsNoTracking()
            .Include(p => p.Store)
            .Where(p => p.TenantId == currentUser.GetTenant() && p.StoreId == query.StoreId)
            .OrderBy(p => p.Name)
            .Select(p => new POSResponse(
                p.Id,
                p.Name,
                p.Description,
                p.Identifier,
                p.Location,
                p.StoreId,
                p.Store.Name,
                p.IsActive,
                p.CreatedOnUtc,
                p.CreatedByUserName))
            .ToListAsync(cancellationToken);
        
        return posTerminals;
    }
}
