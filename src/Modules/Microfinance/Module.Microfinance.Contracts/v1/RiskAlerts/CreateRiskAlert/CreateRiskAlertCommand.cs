using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.RiskAlerts.CreateRiskAlert;

public sealed record CreateRiskAlertCommand(string Name) : ICommand<Guid>;
