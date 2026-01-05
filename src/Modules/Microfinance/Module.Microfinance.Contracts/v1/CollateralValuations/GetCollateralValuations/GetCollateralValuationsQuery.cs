using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollateralValuations.GetCollateralValuations;

public sealed record GetCollateralValuationsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CollateralValuationsPagedResponse>;

public sealed record CollateralValuationsPagedResponse(List<CollateralValuationSummaryDto> Items, int TotalCount, int Page, int PageSize);
