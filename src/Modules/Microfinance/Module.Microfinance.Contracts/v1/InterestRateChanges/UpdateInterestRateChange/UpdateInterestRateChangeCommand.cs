using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InterestRateChanges.UpdateInterestRateChange;

public sealed record UpdateInterestRateChangeCommand(Guid Id, string Name) : ICommand<Guid>;
