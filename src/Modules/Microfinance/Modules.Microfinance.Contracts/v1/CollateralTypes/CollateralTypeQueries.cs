namespace FSH.Modules.Microfinance.Contracts.v1.CollateralTypes;

public record GetCollateralTypeQuery(Guid Id);
public record GetCollateralTypesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record CollateralTypesPagedResponse(List<CollateralTypeSummaryDto> Items, int TotalCount, int Page, int PageSize);
