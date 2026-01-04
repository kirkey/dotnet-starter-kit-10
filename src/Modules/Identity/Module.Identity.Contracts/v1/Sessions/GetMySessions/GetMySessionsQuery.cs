using FSH.Module.Identity.Contracts.DTOs;
using Mediator;

namespace FSH.Module.Identity.Contracts.v1.Sessions.GetMySessions;

public sealed record GetMySessionsQuery : IQuery<List<UserSessionDto>>;
