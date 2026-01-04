using FSH.Module.Identity.Contracts.DTOs;
using Mediator;

namespace FSH.Module.Identity.Contracts.v1.Sessions.GetUserSessions;

public sealed record GetUserSessionsQuery(Guid UserId) : IQuery<List<UserSessionDto>>;
