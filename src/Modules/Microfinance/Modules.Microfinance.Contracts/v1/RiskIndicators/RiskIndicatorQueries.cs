namespace FSH.Modules.Microfinance.Contracts.v1.RiskIndicators;

public record GetRiskIndicatorQuery(Guid Id);
public record GetRiskIndicatorsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record RiskIndicatorsPagedResponse(List<RiskIndicatorSummaryDto> Items, int TotalCount, int Page, int PageSize);
