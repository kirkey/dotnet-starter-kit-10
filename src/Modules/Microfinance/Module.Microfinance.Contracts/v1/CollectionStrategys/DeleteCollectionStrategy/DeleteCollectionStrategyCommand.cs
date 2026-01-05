using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollectionStrategys.DeleteCollectionStrategy;

public sealed record DeleteCollectionStrategyCommand(Guid Id) : ICommand;
