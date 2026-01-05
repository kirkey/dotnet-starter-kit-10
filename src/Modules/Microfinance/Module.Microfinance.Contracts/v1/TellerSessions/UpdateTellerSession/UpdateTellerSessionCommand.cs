using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.TellerSessions.UpdateTellerSession;

public sealed record UpdateTellerSessionCommand(Guid Id, string Name) : ICommand<Guid>;
