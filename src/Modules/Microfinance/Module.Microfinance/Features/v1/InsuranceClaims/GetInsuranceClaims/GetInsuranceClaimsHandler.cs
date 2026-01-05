using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.InsuranceClaims.GetInsuranceClaims;
using FSH.Module.Microfinance.Contracts.v1.InsuranceClaims;

namespace FSH.Module.Microfinance.Features.v1.InsuranceClaims.GetInsuranceClaims;

public class GetInsuranceClaimsHandler(MicrofinanceDbContext context) : IQueryHandler<GetInsuranceClaimsQuery, InsuranceClaimsPagedResponse>
{
    public async ValueTask<InsuranceClaimsPagedResponse> Handle(GetInsuranceClaimsQuery query, CancellationToken ct)
    {
        var queryable = context.InsuranceClaims.AsQueryable();
        
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
            .Select(x => new InsuranceClaimSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new InsuranceClaimsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
