using FSH.Module.Accounting.Contracts.v1.Budgets;
using FSH.Module.Accounting.Contracts.v1.Budgets.GetListBudget;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Budgets.GetBudgets;

/// <summary>
/// Handler for listing budgets with pagination and filtering.
/// </summary>
/// <remarks>
/// Responsibility: Apply SearchTerm and IsActive filters, order by CreatedOnUtc DESC, and return paged summaries.
/// 
/// Returned Fields (BudgetSummaryDto): Id, Name, IsActive
/// 
/// Use Cases: Budget management dashboard, quick search and selection
/// 
/// Permissions: Requires Budget.Search/View
/// 
/// Exceptions: None; returns empty list if no matches
/// </remarks>
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
