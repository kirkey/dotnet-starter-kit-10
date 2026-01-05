using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LegalActions.UpdateLegalAction;

public sealed record UpdateLegalActionCommand(Guid Id, string Name) : ICommand<Guid>;
