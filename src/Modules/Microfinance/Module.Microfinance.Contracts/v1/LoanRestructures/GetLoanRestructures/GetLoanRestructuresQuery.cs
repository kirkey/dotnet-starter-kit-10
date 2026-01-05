using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanRestructures.GetLoanRestructures;

public sealed record GetLoanRestructuresQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<LoanRestructuresPagedResponse>;

public sealed record LoanRestructuresPagedResponse(List<LoanRestructureSummaryDto> Items, int TotalCount, int Page, int PageSize);
