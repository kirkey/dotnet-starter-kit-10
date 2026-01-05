using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollectionStrategys.UpdateCollectionStrategy;

public sealed record UpdateCollectionStrategyCommand(Guid Id, string Name) : ICommand<Guid>;
