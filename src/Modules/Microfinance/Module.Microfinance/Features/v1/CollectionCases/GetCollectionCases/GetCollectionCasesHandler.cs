using FSH.Module.Microfinance.Contracts.v1.CollectionCases;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CollectionCases.GetCollectionCases;

namespace FSH.Module.Microfinance.Features.v1.CollectionCases.GetCollectionCases;

public class GetCollectionCasesHandler(MicrofinanceDbContext context) : IQueryHandler<GetCollectionCasesQuery, CollectionCasesPagedResponse>
{
    public async ValueTask<CollectionCasesPagedResponse> Handle(GetCollectionCasesQuery query, CancellationToken ct)
    {
        var queryable = context.CollectionCases.AsQueryable();
        
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
            .Select(x => new CollectionCaseSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new CollectionCasesPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
