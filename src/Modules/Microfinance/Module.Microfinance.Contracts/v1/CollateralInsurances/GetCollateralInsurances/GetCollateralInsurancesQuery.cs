using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollateralInsurances.GetCollateralInsurances;

public sealed record GetCollateralInsurancesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CollateralInsurancesPagedResponse>;

public sealed record CollateralInsurancesPagedResponse(List<CollateralInsuranceSummaryDto> Items, int TotalCount, int Page, int PageSize);
