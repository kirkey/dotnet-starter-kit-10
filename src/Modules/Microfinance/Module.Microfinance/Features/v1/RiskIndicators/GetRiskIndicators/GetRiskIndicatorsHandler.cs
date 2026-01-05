using FSH.Module.Microfinance.Contracts.v1.RiskIndicators;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.RiskIndicators.GetRiskIndicators;

namespace FSH.Module.Microfinance.Features.v1.RiskIndicators.GetRiskIndicators;

public class GetRiskIndicatorsHandler(MicrofinanceDbContext context) : IQueryHandler<GetRiskIndicatorsQuery, RiskIndicatorsPagedResponse>
{
    public async ValueTask<RiskIndicatorsPagedResponse> Handle(GetRiskIndicatorsQuery query, CancellationToken ct)
    {
        var queryable = context.RiskIndicators.AsQueryable();
        
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
            .Select(x => new RiskIndicatorSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new RiskIndicatorsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
