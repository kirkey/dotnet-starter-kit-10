using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.PromiseToPays.GetPromiseToPays;
using FSH.Module.Microfinance.Contracts.v1.PromiseToPays;

namespace FSH.Module.Microfinance.Features.v1.PromiseToPays.GetPromiseToPays;

public class GetPromiseToPaysHandler(MicrofinanceDbContext context) : IQueryHandler<GetPromiseToPaysQuery, PromiseToPaysPagedResponse>
{
    public async ValueTask<PromiseToPaysPagedResponse> Handle(GetPromiseToPaysQuery query, CancellationToken ct)
    {
        var queryable = context.PromiseToPays.AsQueryable();
        
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
            .Select(x => new PromiseToPaySummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new PromiseToPaysPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
