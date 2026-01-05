using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.TellerSessions.CreateTellerSession;

public sealed record CreateTellerSessionCommand(string Name) : ICommand<Guid>;
