using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Consumption.GetConsumption;

public sealed record GetConsumptionQuery(Guid Id) : IQuery<ConsumptionDto>;
