using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollectionStrategys.CreateCollectionStrategy;

public sealed record CreateCollectionStrategyCommand(string Name) : ICommand<Guid>;
