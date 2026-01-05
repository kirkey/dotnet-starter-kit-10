using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.RiskIndicators.GetRiskIndicators;

public sealed record GetRiskIndicatorsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<RiskIndicatorsPagedResponse>;

public sealed record RiskIndicatorsPagedResponse(List<RiskIndicatorSummaryDto> Items, int TotalCount, int Page, int PageSize);
