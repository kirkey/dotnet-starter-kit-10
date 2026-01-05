using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.RiskAlerts.GetRiskAlert;

public sealed record GetRiskAlertQuery(Guid Id) : IQuery<RiskAlertDto>;
