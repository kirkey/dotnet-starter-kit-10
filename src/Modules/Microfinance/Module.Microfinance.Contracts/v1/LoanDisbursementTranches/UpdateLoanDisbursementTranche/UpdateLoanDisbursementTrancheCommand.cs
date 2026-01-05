using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanDisbursementTranches.UpdateLoanDisbursementTranche;

public sealed record UpdateLoanDisbursementTrancheCommand(Guid Id, string Name) : ICommand<Guid>;
