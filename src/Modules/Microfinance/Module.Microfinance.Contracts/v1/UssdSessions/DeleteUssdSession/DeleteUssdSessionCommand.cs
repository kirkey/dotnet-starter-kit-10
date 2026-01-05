using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.UssdSessions.DeleteUssdSession;

public sealed record DeleteUssdSessionCommand(Guid Id) : ICommand;
