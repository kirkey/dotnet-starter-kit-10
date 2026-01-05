using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.AmlAlerts.GetAmlAlerts;

public sealed record GetAmlAlertsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<AmlAlertsPagedResponse>;
