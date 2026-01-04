namespace FSH.Module.Microfinance.Contracts.v1.LoanApplications;

public record GetLoanApplicationQuery(Guid Id);
public record GetLoanApplicationsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record LoanApplicationsPagedResponse(List<LoanApplicationSummaryDto> Items, int TotalCount, int Page, int PageSize);
