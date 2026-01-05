using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollateralValuations.GetCollateralValuation;

public sealed record GetCollateralValuationQuery(Guid Id) : IQuery<CollateralValuationDto>;
