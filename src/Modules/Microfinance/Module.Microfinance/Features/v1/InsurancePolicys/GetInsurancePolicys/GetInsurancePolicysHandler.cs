using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.InsurancePolicys.GetInsurancePolicys;
using FSH.Module.Microfinance.Contracts.v1.InsurancePolicys;

namespace FSH.Module.Microfinance.Features.v1.InsurancePolicys.GetInsurancePolicys;

public class GetInsurancePolicysHandler(MicrofinanceDbContext context) : IQueryHandler<GetInsurancePolicysQuery, InsurancePolicysPagedResponse>
{
    public async ValueTask<InsurancePolicysPagedResponse> Handle(GetInsurancePolicysQuery query, CancellationToken ct)
    {
        var queryable = context.InsurancePolicys.AsQueryable();
        
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
            .Select(x => new InsurancePolicySummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new InsurancePolicysPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
