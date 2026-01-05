using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanCollaterals.DeleteLoanCollateral;

public sealed record DeleteLoanCollateralCommand(Guid Id) : ICommand;
