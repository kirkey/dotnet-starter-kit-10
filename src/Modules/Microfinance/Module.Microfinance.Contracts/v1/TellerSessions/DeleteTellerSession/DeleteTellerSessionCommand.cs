using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.TellerSessions.DeleteTellerSession;

public sealed record DeleteTellerSessionCommand(Guid Id) : ICommand;
