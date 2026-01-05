using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Consumption.CreateConsumption;

public sealed record CreateConsumptionCommand(string Name, string? Description = null) : ICommand<Guid>;