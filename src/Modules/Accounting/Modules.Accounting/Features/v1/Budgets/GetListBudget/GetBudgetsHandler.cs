using FSH.Modules.Accounting.Contracts.v1.Budgets;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.Budgets.GetBudgets;

public record GetBudgetsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<BudgetsPagedResponse>;

public record BudgetsPagedResponse(
    List<BudgetSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetBudgetsHandler(AccountingDbContext context) 
    : IQueryHandler<GetBudgetsQuery, BudgetsPagedResponse>
{
    public async ValueTask<BudgetsPagedResponse> Handle(GetBudgetsQuery query, CancellationToken ct)
    {
        var queryable = context.Budgets.AsQueryable();
        
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
            .Select(x => new BudgetSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new BudgetsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
