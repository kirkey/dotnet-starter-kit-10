using FSH.Module.Microfinance.Contracts.v1.LoanApplications;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.LoanApplications.GetLoanApplications;

public record GetLoanApplicationsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<LoanApplicationsPagedResponse>;

public class GetLoanApplicationsHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanApplicationsQuery, LoanApplicationsPagedResponse>
{
    public async ValueTask<LoanApplicationsPagedResponse> Handle(GetLoanApplicationsQuery query, CancellationToken ct)
    {
        var queryable = context.LoanApplications.AsQueryable();
        
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
            .Select(x => new LoanApplicationSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new LoanApplicationsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
