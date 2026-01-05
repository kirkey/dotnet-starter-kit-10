using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollectionActions.UpdateCollectionAction;

public sealed record UpdateCollectionActionCommand(Guid Id, string Name) : ICommand<Guid>;
