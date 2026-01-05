using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InterestRateChanges.CreateInterestRateChange;

public sealed record CreateInterestRateChangeCommand(string Name) : ICommand<Guid>;
