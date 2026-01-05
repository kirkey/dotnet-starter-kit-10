using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.RateSchedules.DeleteRateSchedule;

public sealed record DeleteRateScheduleCommand(Guid Id) : ICommand;