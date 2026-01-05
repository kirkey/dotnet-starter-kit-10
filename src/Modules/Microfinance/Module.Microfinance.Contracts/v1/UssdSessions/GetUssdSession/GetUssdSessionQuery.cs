using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.UssdSessions.GetUssdSession;

public sealed record GetUssdSessionQuery(Guid Id) : IQuery<UssdSessionDto>;
