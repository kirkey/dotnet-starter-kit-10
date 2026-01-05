using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanGuarantors.GetLoanGuarantors;

public sealed record GetLoanGuarantorsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<LoanGuarantorsPagedResponse>;
