using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollectionActions.DeleteCollectionAction;

public sealed record DeleteCollectionActionCommand(Guid Id) : ICommand;
