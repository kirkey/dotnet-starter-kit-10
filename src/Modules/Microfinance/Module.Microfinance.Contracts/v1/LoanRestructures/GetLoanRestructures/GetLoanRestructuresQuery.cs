using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanRestructures.GetLoanRestructures;

public sealed record GetLoanRestructuresQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<LoanRestructuresPagedResponse>;
