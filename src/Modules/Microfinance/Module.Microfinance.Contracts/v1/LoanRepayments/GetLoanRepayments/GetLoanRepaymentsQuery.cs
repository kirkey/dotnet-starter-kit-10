using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanRepayments.GetLoanRepayments;

public sealed record GetLoanRepaymentsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<LoanRepaymentsPagedResponse>;

public sealed record LoanRepaymentsPagedResponse(List<LoanRepaymentSummaryDto> Items, int TotalCount, int Page, int PageSize);
