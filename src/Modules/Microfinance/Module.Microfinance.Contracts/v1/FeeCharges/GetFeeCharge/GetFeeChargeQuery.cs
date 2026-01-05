using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FeeCharges.GetFeeCharge;

public sealed record GetFeeChargeQuery(Guid Id) : IQuery<FeeChargeDto>;
