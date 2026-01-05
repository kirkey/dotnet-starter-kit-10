using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FeeCharges.UpdateFeeCharge;

public sealed record UpdateFeeChargeCommand(Guid Id, string Name) : ICommand<Guid>;
