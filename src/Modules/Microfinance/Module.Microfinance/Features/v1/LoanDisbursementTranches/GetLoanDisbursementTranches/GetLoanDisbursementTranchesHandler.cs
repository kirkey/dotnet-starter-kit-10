using FSH.Module.Microfinance.Contracts.v1.LoanDisbursementTranches;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.LoanDisbursementTranches.GetLoanDisbursementTranches;

public record GetLoanDisbursementTranchesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<LoanDisbursementTranchesPagedResponse>;

public class GetLoanDisbursementTranchesHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanDisbursementTranchesQuery, LoanDisbursementTranchesPagedResponse>
{
    public async ValueTask<LoanDisbursementTranchesPagedResponse> Handle(GetLoanDisbursementTranchesQuery query, CancellationToken ct)
    {
        var queryable = context.LoanDisbursementTranches.AsQueryable();
        
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
            .Select(x => new LoanDisbursementTrancheSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new LoanDisbursementTranchesPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
