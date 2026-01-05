using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.UssdSessions.CreateUssdSession;

public sealed record CreateUssdSessionCommand(string Name) : ICommand<Guid>;
