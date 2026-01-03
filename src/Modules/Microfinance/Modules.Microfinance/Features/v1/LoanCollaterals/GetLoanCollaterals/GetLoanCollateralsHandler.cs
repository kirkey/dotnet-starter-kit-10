using FSH.Modules.Microfinance.Contracts.v1.LoanCollaterals;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.LoanCollaterals.GetLoanCollaterals;

public record GetLoanCollateralsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<LoanCollateralsPagedResponse>;

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
