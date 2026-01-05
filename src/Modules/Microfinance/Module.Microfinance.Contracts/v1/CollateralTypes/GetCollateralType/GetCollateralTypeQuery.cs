using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollateralTypes.GetCollateralType;

public sealed record GetCollateralTypeQuery(Guid Id) : IQuery<CollateralTypeDto>;
