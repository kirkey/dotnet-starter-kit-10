using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CollectionStrategys.GetCollectionStrategys;
using FSH.Module.Microfinance.Contracts.v1.CollectionStrategys;

namespace FSH.Module.Microfinance.Features.v1.CollectionStrategys.GetCollectionStrategys;

public class GetCollectionStrategysHandler(MicrofinanceDbContext context) : IQueryHandler<GetCollectionStrategysQuery, CollectionStrategysPagedResponse>
{
    public async ValueTask<CollectionStrategysPagedResponse> Handle(GetCollectionStrategysQuery query, CancellationToken ct)
    {
        var queryable = context.CollectionStrategys.AsQueryable();
        
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
            .Select(x => new CollectionStrategySummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new CollectionStrategysPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
