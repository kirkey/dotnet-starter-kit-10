using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FeeWaivers.GetFeeWaiver;

public sealed record GetFeeWaiverQuery(Guid Id) : IQuery<FeeWaiverDto>;
