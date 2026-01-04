using FSH.Module.Microfinance.Contracts.v1.Loans;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.Loans.GetLoans;

public record GetLoansQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<LoansPagedResponse>;

public class GetLoansHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoansQuery, LoansPagedResponse>
{
    public async ValueTask<LoansPagedResponse> Handle(GetLoansQuery query, CancellationToken ct)
    {
        var queryable = context.Loans.AsQueryable();
        
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
            .Select(x => new LoanSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new LoansPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
