namespace FSH.Module.Microfinance.Contracts.v1.RiskAlerts;

public record RiskAlertsPagedResponse(List<RiskAlertSummaryDto> Items, int TotalCount, int Page, int PageSize);
