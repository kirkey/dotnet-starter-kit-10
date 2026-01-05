using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.RiskAlerts.UpdateRiskAlert;

public sealed record UpdateRiskAlertCommand(Guid Id, string Name) : ICommand<Guid>;
