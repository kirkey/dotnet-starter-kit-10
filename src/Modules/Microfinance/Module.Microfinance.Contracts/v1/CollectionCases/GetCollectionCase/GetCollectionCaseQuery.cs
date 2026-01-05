using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollectionCases.GetCollectionCase;

public sealed record GetCollectionCaseQuery(Guid Id) : IQuery<CollectionCaseDto>;
