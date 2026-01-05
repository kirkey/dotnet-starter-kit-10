using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.RiskIndicators.DeleteRiskIndicator;

public sealed record DeleteRiskIndicatorCommand(Guid Id) : ICommand;
