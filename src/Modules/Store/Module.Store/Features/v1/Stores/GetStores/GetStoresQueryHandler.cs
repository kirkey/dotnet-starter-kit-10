using FSH.Framework.Core.Context;
using FSH.Module.Store.Contracts.v1.Stores;
using FSH.Module.Store.Data;
using Mediator;

namespace FSH.Module.Store.Features.v1.Stores.GetStores;

public sealed class GetStoresQueryHandler(
    StoreDbContext context,
    ICurrentUser currentUser)
    : IQueryHandler<GetStoresQuery, List<StoreResponse>>
{
    public async ValueTask<List<StoreResponse>> Handle(
        GetStoresQuery query,
        CancellationToken cancellationToken)
    {
        var storeQuery = context.Stores.AsNoTracking()
            .Where(s => s.TenantId == currentUser.GetTenant());
        
        if (!query.IncludeInactive)
        {
            storeQuery = storeQuery.Where(s => s.IsActive);
        }
        
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var searchTerm = query.Search.Trim().ToLower();
            storeQuery = storeQuery.Where(s => s.Name.ToLower().Contains(searchTerm));
        }
        
        var stores = await storeQuery
            .OrderBy(s => s.Name)
            .Select(s => new StoreResponse(
                s.Id,
                s.Name,
                s.Description,
                s.Address,
                s.City,
                s.State,
                s.PostalCode,
                s.Phone,
                s.Email,
                s.IsActive,
                s.CreatedOnUtc,
                s.CreatedByUserName))
            .ToListAsync(cancellationToken);
        
        return stores;
    }
}
