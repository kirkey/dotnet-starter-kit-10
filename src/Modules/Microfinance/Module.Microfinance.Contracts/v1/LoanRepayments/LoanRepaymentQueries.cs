namespace FSH.Module.Microfinance.Contracts.v1.LoanRepayments;

public record GetLoanRepaymentQuery(Guid Id);
public record GetLoanRepaymentsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record LoanRepaymentsPagedResponse(List<LoanRepaymentSummaryDto> Items, int TotalCount, int Page, int PageSize);
