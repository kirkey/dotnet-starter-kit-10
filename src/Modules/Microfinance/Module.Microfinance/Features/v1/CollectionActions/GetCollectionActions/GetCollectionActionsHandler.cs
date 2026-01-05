using FSH.Module.Microfinance.Contracts.v1.CollectionActions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CollectionActions.GetCollectionActions;

namespace FSH.Module.Microfinance.Features.v1.CollectionActions.GetCollectionActions;

public class GetCollectionActionsHandler(MicrofinanceDbContext context) : IQueryHandler<GetCollectionActionsQuery, CollectionActionsPagedResponse>
{
    public async ValueTask<CollectionActionsPagedResponse> Handle(GetCollectionActionsQuery query, CancellationToken ct)
    {
        var queryable = context.CollectionActions.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(x => x.Name.Contains(query.SearchTerm));
        }
        
        if (query.IsActive.HasValue)
        {
            queryable = queryable.Where(x => x.IsActive == query.IsActive.Value);
        }
        
        var totalCount = await queryable.CountAsync(ct);
        
        var items = await queryable
            .OrderByDescending(x => x.CreatedOnUtc)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new CollectionActionSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new CollectionActionsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
