using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.UssdSessions.GetUssdSessions;
using FSH.Module.Microfinance.Contracts.v1.UssdSessions;

namespace FSH.Module.Microfinance.Features.v1.UssdSessions.GetUssdSessions;

public class GetUssdSessionsHandler(MicrofinanceDbContext context) : IQueryHandler<GetUssdSessionsQuery, UssdSessionsPagedResponse>
{
    public async ValueTask<UssdSessionsPagedResponse> Handle(GetUssdSessionsQuery query, CancellationToken ct)
    {
        var queryable = context.UssdSessions.AsQueryable();
        
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
            .Select(x => new UssdSessionSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new UssdSessionsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
