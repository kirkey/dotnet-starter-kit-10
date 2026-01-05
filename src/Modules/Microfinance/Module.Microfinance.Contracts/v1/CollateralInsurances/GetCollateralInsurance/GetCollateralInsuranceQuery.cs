using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollateralInsurances.GetCollateralInsurance;

public sealed record GetCollateralInsuranceQuery(Guid Id) : IQuery<CollateralInsuranceDto>;
