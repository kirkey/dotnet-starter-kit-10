namespace FSH.Modules.Microfinance.Contracts.v1.CollateralInsurances;

public record GetCollateralInsuranceQuery(Guid Id);
public record GetCollateralInsurancesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record CollateralInsurancesPagedResponse(List<CollateralInsuranceSummaryDto> Items, int TotalCount, int Page, int PageSize);
