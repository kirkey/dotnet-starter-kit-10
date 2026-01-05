using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FeeCharges.DeleteFeeCharge;

public sealed record DeleteFeeChargeCommand(Guid Id) : ICommand;
