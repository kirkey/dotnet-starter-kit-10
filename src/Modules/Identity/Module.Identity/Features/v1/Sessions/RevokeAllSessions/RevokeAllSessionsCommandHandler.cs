using FSH.Framework.Core.Context;
using FSH.Module.Identity.Contracts.Services;
using FSH.Module.Identity.Contracts.v1.Sessions.RevokeAllSessions;
using Mediator;

namespace FSH.Module.Identity.Features.v1.Sessions.RevokeAllSessions;

public sealed class RevokeAllSessionsCommandHandler(ISessionService sessionService, ICurrentUser currentUser)
    : ICommandHandler<RevokeAllSessionsCommand, int>
{
    public async ValueTask<int> Handle(RevokeAllSessionsCommand command, CancellationToken cancellationToken)
    {
        string userId = currentUser.GetUserId().ToString();
        return await sessionService.RevokeAllSessionsAsync(
            userId,
            userId,
            command.ExceptSessionId,
            "User requested logout from all devices",
            cancellationToken);
    }
}
