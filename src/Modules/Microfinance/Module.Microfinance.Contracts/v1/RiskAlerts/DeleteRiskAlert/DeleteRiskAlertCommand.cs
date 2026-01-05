using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.RiskAlerts.DeleteRiskAlert;

public sealed record DeleteRiskAlertCommand(Guid Id) : ICommand;
