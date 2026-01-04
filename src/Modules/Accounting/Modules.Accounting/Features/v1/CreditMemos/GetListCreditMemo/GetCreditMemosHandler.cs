using FSH.Modules.Accounting.Contracts.v1.CreditMemos;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.CreditMemos.GetCreditMemos;

public record GetCreditMemosQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<CreditMemosPagedResponse>;

public record CreditMemosPagedResponse(
    List<CreditMemoSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetCreditMemosHandler(AccountingDbContext context) 
    : IQueryHandler<GetCreditMemosQuery, CreditMemosPagedResponse>
{
    public async ValueTask<CreditMemosPagedResponse> Handle(GetCreditMemosQuery query, CancellationToken ct)
    {
        var queryable = context.CreditMemos.AsQueryable();
        
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
            .Select(x => new CreditMemoSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new CreditMemosPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
