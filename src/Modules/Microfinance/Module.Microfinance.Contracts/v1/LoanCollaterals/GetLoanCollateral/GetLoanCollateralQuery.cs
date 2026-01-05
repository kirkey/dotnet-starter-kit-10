using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanCollaterals.GetLoanCollateral;

public sealed record GetLoanCollateralQuery(Guid Id) : IQuery<LoanCollateralDto>;
