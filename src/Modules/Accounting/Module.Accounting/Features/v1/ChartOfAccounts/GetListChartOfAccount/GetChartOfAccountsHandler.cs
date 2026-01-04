using FSH.Module.Accounting.Contracts.v1.ChartOfAccounts;
using FSH.Module.Accounting.Contracts.v1.ChartOfAccounts.GetChartOfAccounts;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.ChartOfAccounts.GetChartOfAccounts;

/// <summary>
/// Handler for retrieving paginated list of Chart of Accounts.
/// 
/// **Responsibility:**
/// Queries the database for Chart of Accounts with filtering, sorting, and pagination.
/// Returns a summary DTO for each account (lighter than full entity).
/// 
/// **Execution Flow:**
/// 1. Build base queryable from DbSet
/// 2. Apply SearchTerm filter (AccountCode OR AccountName contains)
/// 3. Apply IsActive filter (if specified)
/// 4. Apply AccountType filter (if specified)
/// 5. Count total matching records
/// 6. Sort by AccountCode ascending
/// 7. Skip and take for pagination
/// 8. Project to summary DTOs
/// 9. Return paged response with items and metadata
/// 
/// **Filtering Logic:**
/// - SearchTerm: Case-sensitive substring match on AccountCode or AccountName
/// - IsActive: Exact match on boolean flag
/// - AccountType: Exact match on account type string
/// - Multiple filters: All specified filters applied (AND logic)
/// 
/// **Sorting:**
/// Primary: AccountCode (ascending)
/// 
/// **Pagination:**
/// - Page: 1-indexed
/// - PageSize: Records per page
/// - Skip calculation: (Page - 1) * PageSize
/// - TotalCount: Full count before pagination
/// 
/// **Returned Fields per Account:**
/// - Id, AccountCode, AccountName
/// - AccountType, Balance
/// - IsActive
/// 
/// **Permissions:**
/// Requires: Accounting.ChartOfAccount.View
/// </summary>
public class GetChartOfAccountsHandler(AccountingDbContext context) 
    : IQueryHandler<GetChartOfAccountsQuery, ChartOfAccountsPagedResponse>
{
    /// <summary>
    /// Handles the GetChartOfAccountsQuery to retrieve a paginated list.
    /// </summary>
    /// <param name="query">The query containing pagination and filter parameters</param>
    /// <param name="ct">Cancellation token for the operation</param>
    /// <returns>Paged response with account summaries and total count</returns>
    public async ValueTask<ChartOfAccountsPagedResponse> Handle(GetChartOfAccountsQuery query, CancellationToken ct)
    {
        var queryable = context.ChartOfAccounts.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(x => 
                x.AccountCode.Contains(query.SearchTerm) || 
                x.AccountName.Contains(query.SearchTerm));
        }
        
        if (query.IsActive.HasValue)
        {
            queryable = queryable.Where(x => x.IsActive == query.IsActive.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(query.AccountType))
        {
            queryable = queryable.Where(x => x.AccountType == query.AccountType);
        }
        
        var totalCount = await queryable.CountAsync(ct);
        
        var items = await queryable
            .OrderBy(x => x.AccountCode)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new ChartOfAccountSummaryDto(
                x.Id,
                x.AccountCode,
                x.AccountName,
                x.AccountType,
                x.Balance,
                x.IsActive))
            .ToListAsync(ct);
        
        return new ChartOfAccountsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
