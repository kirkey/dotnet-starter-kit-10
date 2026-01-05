using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.RiskCategorys.GetRiskCategorys;
using FSH.Module.Microfinance.Contracts.v1.RiskCategorys;

namespace FSH.Module.Microfinance.Features.v1.RiskCategorys.GetRiskCategorys;

public class GetRiskCategorysHandler(MicrofinanceDbContext context) : IQueryHandler<GetRiskCategorysQuery, RiskCategorysPagedResponse>
{
    public async ValueTask<RiskCategorysPagedResponse> Handle(GetRiskCategorysQuery query, CancellationToken ct)
    {
        var queryable = context.RiskCategorys.AsQueryable();
        
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
            .Select(x => new RiskCategorySummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new RiskCategorysPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
