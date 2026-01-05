using FSH.Module.Accounting.Contracts.v1.ChartOfAccounts;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.ChartOfAccounts.GetChartOfAccounts;

/// <summary>
/// Get Chart of Accounts list query.
/// 
/// **Purpose:**
/// Encapsulates the request to retrieve a paginated list of Chart of Accounts with filtering and sorting.
/// 
/// **Parameters:**
/// - Page: Page number for pagination (default: 1)
/// - PageSize: Number of items per page (default: 10)
/// - SearchTerm: Text search across AccountCode and AccountName
/// - IsActive: Filter by active status (null = all)
/// - AccountType: Filter by account type (Asset, Liability, Equity, Revenue, Expense, etc.)
/// 
/// **Multi-Tenancy:**
/// Tenant context is automatically applied via query filters.
/// 
/// **Filtering:**
/// Supports multiple filter combinations:
/// - Text search on AccountCode and AccountName
/// - Active/Inactive status filtering
/// - Account type classification filtering
/// </summary>
public record GetChartOfAccountsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null,
    string? AccountType = null) : IQuery<ChartOfAccountsPagedResponse>;

/// <summary>
/// Response containing paginated Chart of Accounts list.
/// </summary>
public record ChartOfAccountsPagedResponse(
    List<ChartOfAccountSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

/// <summary>
/// Summary DTO for Chart of Account in list view.
/// </summary>
public record ChartOfAccountSummaryDto(
    Guid Id,
    string AccountCode,
    string AccountName,
    string AccountType,
    decimal Balance,
    bool IsActive);