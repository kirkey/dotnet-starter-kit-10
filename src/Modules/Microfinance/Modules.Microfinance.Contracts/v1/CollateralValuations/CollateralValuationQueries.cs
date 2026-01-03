namespace FSH.Modules.Microfinance.Contracts.v1.CollateralValuations;

public record GetCollateralValuationQuery(Guid Id);
public record GetCollateralValuationsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record CollateralValuationsPagedResponse(List<CollateralValuationSummaryDto> Items, int TotalCount, int Page, int PageSize);
