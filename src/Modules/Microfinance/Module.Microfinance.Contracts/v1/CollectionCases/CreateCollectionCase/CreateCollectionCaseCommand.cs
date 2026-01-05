using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollectionCases.CreateCollectionCase;

public sealed record CreateCollectionCaseCommand(string Name) : ICommand<Guid>;
