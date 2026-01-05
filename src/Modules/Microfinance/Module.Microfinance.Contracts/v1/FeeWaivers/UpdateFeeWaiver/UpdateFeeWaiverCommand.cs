using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FeeWaivers.UpdateFeeWaiver;

public sealed record UpdateFeeWaiverCommand(Guid Id, string Name) : ICommand<Guid>;
