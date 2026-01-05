using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InterestRateChanges.DeleteInterestRateChange;

public sealed record DeleteInterestRateChangeCommand(Guid Id) : ICommand;
