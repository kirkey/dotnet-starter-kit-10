using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Meters.GetMeter;

public sealed record GetMeterQuery(Guid Id) : IQuery<MeterDto>;
