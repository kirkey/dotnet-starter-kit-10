using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Consumption.DeleteConsumption;

public sealed record DeleteConsumptionCommand(Guid Id) : ICommand;