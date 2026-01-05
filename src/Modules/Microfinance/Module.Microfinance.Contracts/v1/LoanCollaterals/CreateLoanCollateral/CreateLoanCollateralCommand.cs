using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanCollaterals.CreateLoanCollateral;

public sealed record CreateLoanCollateralCommand(string Name) : ICommand<Guid>;
