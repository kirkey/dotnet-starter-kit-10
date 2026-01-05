using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FeeWaivers.DeleteFeeWaiver;

public sealed record DeleteFeeWaiverCommand(Guid Id) : ICommand;
