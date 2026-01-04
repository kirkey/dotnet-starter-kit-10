using FSH.Module.Microfinance.Contracts.v1.LoanRepayments;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.LoanRepayments.GetLoanRepayments;

public record GetLoanRepaymentsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<LoanRepaymentsPagedResponse>;

public class GetLoanRepaymentsHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanRepaymentsQuery, LoanRepaymentsPagedResponse>
{
    public async ValueTask<LoanRepaymentsPagedResponse> Handle(GetLoanRepaymentsQuery query, CancellationToken ct)
    {
        var queryable = context.LoanRepayments.AsQueryable();
        
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
            .Select(x => new LoanRepaymentSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new LoanRepaymentsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
