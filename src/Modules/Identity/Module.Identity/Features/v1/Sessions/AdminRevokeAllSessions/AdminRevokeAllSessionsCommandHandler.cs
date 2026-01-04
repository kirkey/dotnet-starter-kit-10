using FSH.Framework.Core.Context;
using FSH.Module.Identity.Contracts.Services;
using FSH.Module.Identity.Contracts.v1.Sessions.AdminRevokeAllSessions;
using Mediator;

namespace FSH.Module.Identity.Features.v1.Sessions.AdminRevokeAllSessions;

public sealed class AdminRevokeAllSessionsCommandHandler(ISessionService sessionService, ICurrentUser currentUser)
    : ICommandHandler<AdminRevokeAllSessionsCommand, int>
{
    public async ValueTask<int> Handle(AdminRevokeAllSessionsCommand command, CancellationToken cancellationToken)
    {
        string adminId = currentUser.GetUserId().ToString();
        return await sessionService.RevokeAllSessionsForAdminAsync(
            command.UserId.ToString(),
            adminId,
            command.Reason ?? "Revoked by administrator",
            cancellationToken);
    }
}
