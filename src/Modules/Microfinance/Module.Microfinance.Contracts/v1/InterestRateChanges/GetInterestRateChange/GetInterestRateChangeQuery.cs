using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InterestRateChanges.GetInterestRateChange;

public sealed record GetInterestRateChangeQuery(Guid Id) : IQuery<InterestRateChangeDto>;
