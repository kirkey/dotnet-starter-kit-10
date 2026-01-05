using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanDisbursementTranches.DeleteLoanDisbursementTranche;

public sealed record DeleteLoanDisbursementTrancheCommand(Guid Id) : ICommand;
