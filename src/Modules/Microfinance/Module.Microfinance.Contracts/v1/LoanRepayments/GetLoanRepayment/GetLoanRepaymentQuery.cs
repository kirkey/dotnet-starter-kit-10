using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanRepayments.GetLoanRepayment;

public sealed record GetLoanRepaymentQuery(Guid Id) : IQuery<LoanRepaymentDto>;
