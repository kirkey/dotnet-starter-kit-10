using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.RiskIndicators.UpdateRiskIndicator;

public sealed record UpdateRiskIndicatorCommand(Guid Id, string Name) : ICommand<Guid>;
