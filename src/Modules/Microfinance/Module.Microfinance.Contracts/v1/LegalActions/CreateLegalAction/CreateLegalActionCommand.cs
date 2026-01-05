using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LegalActions.CreateLegalAction;

public sealed record CreateLegalActionCommand(string Name) : ICommand<Guid>;
