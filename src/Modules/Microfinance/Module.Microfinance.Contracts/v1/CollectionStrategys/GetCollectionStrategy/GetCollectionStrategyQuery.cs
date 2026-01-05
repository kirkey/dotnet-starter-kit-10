using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollectionStrategys.GetCollectionStrategy;

public sealed record GetCollectionStrategyQuery(Guid Id) : IQuery<CollectionStrategyDto>;
