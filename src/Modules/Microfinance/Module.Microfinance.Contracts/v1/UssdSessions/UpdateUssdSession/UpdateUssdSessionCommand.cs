using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.UssdSessions.UpdateUssdSession;

public sealed record UpdateUssdSessionCommand(Guid Id, string Name) : ICommand<Guid>;
