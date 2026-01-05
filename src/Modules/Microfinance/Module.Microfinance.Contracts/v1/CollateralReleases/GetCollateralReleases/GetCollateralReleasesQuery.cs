using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollateralReleases.GetCollateralReleases;

public sealed record GetCollateralReleasesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CollateralReleasesPagedResponse>;
