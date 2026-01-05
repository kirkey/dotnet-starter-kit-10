using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Meters.DeleteMeter;

public record DeleteMeterCommand(Guid Id) : ICommand<Guid>;