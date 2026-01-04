using FSH.Module.Accounting.Contracts.v1.Budgets;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Budgets.GetBudgets;

/// <summary>
/// Query to retrieve a paginated list of budgets with optional name search and active filter.
/// </summary>
/// <param name="Page">Page number for pagination (1-based)</param>
/// <param name="PageSize">Items per page</param>
/// <param name="SearchTerm">Optional substring search on budget Name</param>
/// <param name="IsActive">Optional filter by active status</param>
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
