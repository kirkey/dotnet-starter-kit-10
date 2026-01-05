using FSH.Module.Microfinance.Contracts.v1.AmlAlerts;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.AmlAlerts.GetAmlAlerts;

namespace FSH.Module.Microfinance.Features.v1.AmlAlerts.GetAmlAlerts;

public class GetAmlAlertsHandler(MicrofinanceDbContext context) : IQueryHandler<GetAmlAlertsQuery, AmlAlertsPagedResponse>
{
    public async ValueTask<AmlAlertsPagedResponse> Handle(GetAmlAlertsQuery query, CancellationToken ct)
    {
        var queryable = context.AmlAlerts.AsQueryable();
        
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
            .Select(x => new AmlAlertSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new AmlAlertsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
