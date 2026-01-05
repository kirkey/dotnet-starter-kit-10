using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollateralReleases.GetCollateralRelease;

public sealed record GetCollateralReleaseQuery(Guid Id) : IQuery<CollateralReleaseDto>;
