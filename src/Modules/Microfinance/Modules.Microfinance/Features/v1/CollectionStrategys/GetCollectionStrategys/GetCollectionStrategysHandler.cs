using FSH.Modules.Microfinance.Contracts.v1.CollectionStrategys;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CollectionStrategys.GetCollectionStrategys;

public record GetCollectionStrategysQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CollectionStrategysPagedResponse>;

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
