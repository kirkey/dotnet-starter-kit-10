using FSH.Modules.Microfinance.Contracts.v1.TellerSessions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.TellerSessions.GetTellerSessions;

public record GetTellerSessionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<TellerSessionsPagedResponse>;

public class GetTellerSessionsHandler(MicrofinanceDbContext context) : IQueryHandler<GetTellerSessionsQuery, TellerSessionsPagedResponse>
{
    public async ValueTask<TellerSessionsPagedResponse> Handle(GetTellerSessionsQuery query, CancellationToken ct)
    {
        var queryable = context.TellerSessions.AsQueryable();
        
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
            .Select(x => new TellerSessionSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new TellerSessionsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
