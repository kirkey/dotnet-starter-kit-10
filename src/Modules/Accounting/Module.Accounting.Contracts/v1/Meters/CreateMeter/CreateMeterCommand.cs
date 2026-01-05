using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Meters.CreateMeter;

public record CreateMeterCommand(string Name, string? Description, bool IsActive = true) : ICommand<Guid>;