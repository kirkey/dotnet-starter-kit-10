using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanCollaterals.GetLoanCollaterals;
using FSH.Module.Microfinance.Contracts.v1.LoanCollaterals;

namespace FSH.Module.Microfinance.Features.v1.LoanCollaterals.GetLoanCollaterals;

public class GetLoanCollateralsHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanCollateralsQuery, LoanCollateralsPagedResponse>
{
    public async ValueTask<LoanCollateralsPagedResponse> Handle(GetLoanCollateralsQuery query, CancellationToken ct)
    {
        var queryable = context.LoanCollaterals.AsQueryable();
        
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
            .Select(x => new LoanCollateralSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new LoanCollateralsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
