using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.RiskIndicators.GetRiskIndicator;

public sealed record GetRiskIndicatorQuery(Guid Id) : IQuery<RiskIndicatorDto>;
