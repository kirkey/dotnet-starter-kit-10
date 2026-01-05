using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Loans;

/// <summary>
/// Query to get a paginated list of loans.
/// </summary>
public record GetLoansQuery(
    int Page, 
    int PageSize, 
    string? SearchTerm, 
    bool? IsActive) : IQuery<LoansPagedResponse>;

/// <summary>
/// Paginated response for loans query.
/// </summary>
public record LoansPagedResponse(
    List<LoanSummaryDto> Items, 
    int TotalCount, 
    int Page, 
    int PageSize);
