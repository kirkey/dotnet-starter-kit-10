using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollectionActions.GetCollectionAction;

public sealed record GetCollectionActionQuery(Guid Id) : IQuery<CollectionActionDto>;
