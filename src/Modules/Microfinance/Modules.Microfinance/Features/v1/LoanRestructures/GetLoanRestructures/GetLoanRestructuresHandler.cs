using FSH.Modules.Microfinance.Contracts.v1.LoanRestructures;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.LoanRestructures.GetLoanRestructures;

public record GetLoanRestructuresQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<LoanRestructuresPagedResponse>;

public class GetLoanRestructuresHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanRestructuresQuery, LoanRestructuresPagedResponse>
{
    public async ValueTask<LoanRestructuresPagedResponse> Handle(GetLoanRestructuresQuery query, CancellationToken ct)
    {
        var queryable = context.LoanRestructures.AsQueryable();
        
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
            .Select(x => new LoanRestructureSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new LoanRestructuresPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
