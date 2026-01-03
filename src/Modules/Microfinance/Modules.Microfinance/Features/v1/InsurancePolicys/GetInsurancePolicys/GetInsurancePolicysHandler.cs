using FSH.Modules.Microfinance.Contracts.v1.InsurancePolicys;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.InsurancePolicys.GetInsurancePolicys;

public record GetInsurancePolicysQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<InsurancePolicysPagedResponse>;

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
