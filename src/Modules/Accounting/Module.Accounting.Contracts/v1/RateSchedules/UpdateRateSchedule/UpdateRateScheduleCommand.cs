using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.RateSchedules.UpdateRateSchedule;

public sealed record UpdateRateScheduleCommand(Guid Id, string Name, string? Description = null) : ICommand<Guid>;