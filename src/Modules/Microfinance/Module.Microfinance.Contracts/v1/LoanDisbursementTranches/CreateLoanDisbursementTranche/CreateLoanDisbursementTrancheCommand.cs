using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanDisbursementTranches.CreateLoanDisbursementTranche;

public sealed record CreateLoanDisbursementTrancheCommand(string Name) : ICommand<Guid>;
