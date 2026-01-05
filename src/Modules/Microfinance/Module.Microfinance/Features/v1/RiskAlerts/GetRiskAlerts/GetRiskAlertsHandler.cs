using FSH.Module.Microfinance.Contracts.v1.RiskAlerts;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.RiskAlerts.GetRiskAlerts;

namespace FSH.Module.Microfinance.Features.v1.RiskAlerts.GetRiskAlerts;

public class GetRiskAlertsHandler(MicrofinanceDbContext context) : IQueryHandler<GetRiskAlertsQuery, RiskAlertsPagedResponse>
{
    public async ValueTask<RiskAlertsPagedResponse> Handle(GetRiskAlertsQuery query, CancellationToken ct)
    {
        var queryable = context.RiskAlerts.AsQueryable();
        
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
            .Select(x => new RiskAlertSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new RiskAlertsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
