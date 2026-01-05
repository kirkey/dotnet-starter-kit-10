using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanCollaterals.UpdateLoanCollateral;

public sealed record UpdateLoanCollateralCommand(Guid Id, string Name) : ICommand<Guid>;
