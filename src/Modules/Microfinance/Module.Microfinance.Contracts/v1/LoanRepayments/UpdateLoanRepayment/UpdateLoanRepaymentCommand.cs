using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanRepayments.UpdateLoanRepayment;

public sealed record UpdateLoanRepaymentCommand(Guid Id, string Name) : ICommand<Guid>;
