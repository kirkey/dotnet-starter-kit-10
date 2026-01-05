using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollectionCases.DeleteCollectionCase;

public sealed record DeleteCollectionCaseCommand(Guid Id) : ICommand;
