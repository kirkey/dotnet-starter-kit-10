using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LegalActions.DeleteLegalAction;

public sealed record DeleteLegalActionCommand(Guid Id) : ICommand;
