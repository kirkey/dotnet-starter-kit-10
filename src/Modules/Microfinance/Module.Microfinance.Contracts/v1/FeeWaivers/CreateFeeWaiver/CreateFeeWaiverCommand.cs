using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FeeWaivers.CreateFeeWaiver;

public sealed record CreateFeeWaiverCommand(string Name) : ICommand<Guid>;
