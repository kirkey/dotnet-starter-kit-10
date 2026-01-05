using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Consumption.UpdateConsumption;

public sealed record UpdateConsumptionCommand(Guid Id, string Name, string? Description = null) : ICommand<Guid>;