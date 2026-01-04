namespace FSH.Module.Microfinance.Contracts.v1.CollateralReleases;

public record GetCollateralReleaseQuery(Guid Id);
public record GetCollateralReleasesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record CollateralReleasesPagedResponse(List<CollateralReleaseSummaryDto> Items, int TotalCount, int Page, int PageSize);
