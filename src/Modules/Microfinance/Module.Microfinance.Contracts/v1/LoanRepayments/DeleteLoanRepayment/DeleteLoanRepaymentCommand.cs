using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanRepayments.DeleteLoanRepayment;

public sealed record DeleteLoanRepaymentCommand(Guid Id) : ICommand;
