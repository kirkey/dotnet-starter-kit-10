using FSH.Framework.Core.Context;
using FSH.Module.Identity.Contracts.DTOs;
using FSH.Module.Identity.Contracts.Services;
using FSH.Module.Identity.Contracts.v1.Sessions.GetMySessions;
using Mediator;

namespace FSH.Module.Identity.Features.v1.Sessions.GetMySessions;

public sealed class GetMySessionsQueryHandler(ISessionService sessionService, ICurrentUser currentUser)
    : IQueryHandler<GetMySessionsQuery, List<UserSessionDto>>
{
    public async ValueTask<List<UserSessionDto>> Handle(GetMySessionsQuery query, CancellationToken cancellationToken)
    {
        string userId = currentUser.GetUserId().ToString();
        return await sessionService.GetUserSessionsAsync(userId, cancellationToken);
    }
}
