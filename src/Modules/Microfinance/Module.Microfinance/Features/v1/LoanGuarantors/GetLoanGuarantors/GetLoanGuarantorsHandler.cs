using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanGuarantors.GetLoanGuarantors;
using FSH.Module.Microfinance.Contracts.v1.LoanGuarantors;

namespace FSH.Module.Microfinance.Features.v1.LoanGuarantors.GetLoanGuarantors;

public class GetLoanGuarantorsHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanGuarantorsQuery, LoanGuarantorsPagedResponse>
{
    public async ValueTask<LoanGuarantorsPagedResponse> Handle(GetLoanGuarantorsQuery query, CancellationToken ct)
    {
        var queryable = context.LoanGuarantors.AsQueryable();
        
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
            .Select(x => new LoanGuarantorSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new LoanGuarantorsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
