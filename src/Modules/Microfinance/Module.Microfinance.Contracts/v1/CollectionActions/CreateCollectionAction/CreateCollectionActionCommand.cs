using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollectionActions.CreateCollectionAction;

public sealed record CreateCollectionActionCommand(string Name) : ICommand<Guid>;
