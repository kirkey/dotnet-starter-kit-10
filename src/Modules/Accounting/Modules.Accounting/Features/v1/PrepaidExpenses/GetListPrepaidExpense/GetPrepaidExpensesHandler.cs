using FSH.Modules.Accounting.Contracts.v1.PrepaidExpenses;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.PrepaidExpenses.GetPrepaidExpenses;

public record GetPrepaidExpensesQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<PrepaidExpensesPagedResponse>;

public record PrepaidExpensesPagedResponse(
    List<PrepaidExpenseSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetPrepaidExpensesHandler(AccountingDbContext context) 
    : IQueryHandler<GetPrepaidExpensesQuery, PrepaidExpensesPagedResponse>
{
    public async ValueTask<PrepaidExpensesPagedResponse> Handle(GetPrepaidExpensesQuery query, CancellationToken ct)
    {
        var queryable = context.PrepaidExpenses.AsQueryable();
        
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
            .Select(x => new PrepaidExpenseSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new PrepaidExpensesPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
