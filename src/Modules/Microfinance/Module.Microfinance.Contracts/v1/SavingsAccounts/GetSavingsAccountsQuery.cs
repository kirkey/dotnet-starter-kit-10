using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.SavingsAccounts;

/// <summary>
/// Query to get a paginated list of savings accounts.
/// </summary>
public record GetSavingsAccountsQuery(
    int Page, 
    int PageSize, 
    string? SearchTerm, 
    bool? IsActive) : IQuery<SavingsAccountsPagedResponse>;

/// <summary>
/// Paginated response for savings accounts query.
/// </summary>
public record SavingsAccountsPagedResponse(
    List<SavingsAccountSummaryDto> Items, 
    int TotalCount, 
    int Page, 
    int PageSize);
