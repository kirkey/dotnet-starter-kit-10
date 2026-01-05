using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.TellerSessions.GetTellerSession;

public sealed record GetTellerSessionQuery(Guid Id) : IQuery<TellerSessionDto>;
