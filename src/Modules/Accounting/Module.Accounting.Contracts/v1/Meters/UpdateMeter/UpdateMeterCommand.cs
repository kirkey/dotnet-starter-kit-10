using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Meters.UpdateMeter;

public record UpdateMeterCommand(Guid Id, string Name, string? Description, bool IsActive) : ICommand<Guid>;