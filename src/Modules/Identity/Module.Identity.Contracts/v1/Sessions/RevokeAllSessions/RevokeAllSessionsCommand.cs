using Mediator;

namespace FSH.Module.Identity.Contracts.v1.Sessions.RevokeAllSessions;

public sealed record RevokeAllSessionsCommand(Guid? ExceptSessionId = null) : ICommand<int>;
