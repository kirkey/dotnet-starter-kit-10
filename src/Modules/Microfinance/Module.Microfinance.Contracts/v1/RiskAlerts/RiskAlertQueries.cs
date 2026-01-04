namespace FSH.Module.Microfinance.Contracts.v1.RiskAlerts;

public record GetRiskAlertQuery(Guid Id);
public record GetRiskAlertsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record RiskAlertsPagedResponse(List<RiskAlertSummaryDto> Items, int TotalCount, int Page, int PageSize);
