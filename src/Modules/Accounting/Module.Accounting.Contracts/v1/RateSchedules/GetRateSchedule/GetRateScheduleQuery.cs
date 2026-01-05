using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.RateSchedules.GetRateSchedule;

public sealed record GetRateScheduleQuery(Guid Id) : IQuery<RateScheduleDto>;