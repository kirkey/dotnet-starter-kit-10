using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanApplications;

/// <summary>
/// Query to get a paginated list of loan applications.
/// </summary>
public record GetLoanApplicationsQuery(
    int Page, 
    int PageSize, 
    string? SearchTerm, 
    bool? IsActive) : IQuery<LoanApplicationsPagedResponse>;

/// <summary>
/// Paginated response for loan applications query.
/// </summary>
public record LoanApplicationsPagedResponse(
    List<LoanApplicationSummaryDto> Items, 
    int TotalCount, 
    int Page, 
    int PageSize);
