using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FeeCharges.CreateFeeCharge;

public sealed record CreateFeeChargeCommand(string Name) : ICommand<Guid>;
