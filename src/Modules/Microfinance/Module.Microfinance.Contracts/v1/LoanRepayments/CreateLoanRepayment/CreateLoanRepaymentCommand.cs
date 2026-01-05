using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanRepayments.CreateLoanRepayment;

public sealed record CreateLoanRepaymentCommand(string Name) : ICommand<Guid>;
