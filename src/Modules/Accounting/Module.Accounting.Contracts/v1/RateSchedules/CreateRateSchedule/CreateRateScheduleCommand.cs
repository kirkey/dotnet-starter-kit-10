using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.RateSchedules.CreateRateSchedule;

public sealed record CreateRateScheduleCommand(string Name, string? Description = null) : ICommand<Guid>;