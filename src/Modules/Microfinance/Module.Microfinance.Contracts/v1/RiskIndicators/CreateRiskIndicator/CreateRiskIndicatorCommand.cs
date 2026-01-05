using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.RiskIndicators.CreateRiskIndicator;

public sealed record CreateRiskIndicatorCommand(string Name) : ICommand<Guid>;
