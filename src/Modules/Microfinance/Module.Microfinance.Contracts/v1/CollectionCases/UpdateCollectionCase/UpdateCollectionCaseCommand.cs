using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollectionCases.UpdateCollectionCase;

public sealed record UpdateCollectionCaseCommand(Guid Id, string Name) : ICommand<Guid>;
