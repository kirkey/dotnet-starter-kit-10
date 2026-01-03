namespace FSH.Modules.Microfinance.Contracts.v1.AmlAlerts;

public record GetAmlAlertQuery(Guid Id);
public record GetAmlAlertsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record AmlAlertsPagedResponse(List<AmlAlertSummaryDto> Items, int TotalCount, int Page, int PageSize);
