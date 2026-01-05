using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.RiskAlerts.GetRiskAlerts;

public sealed record GetRiskAlertsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<RiskAlertsPagedResponse>;
