using FSH.Framework.Shared.Identity;
using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Identity.Contracts.v1.Sessions.AdminRevokeSession;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Identity.Features.v1.Sessions.AdminRevokeSession;

public static class AdminRevokeSessionEndpoint
{
    internal static RouteHandlerBuilder MapAdminRevokeSessionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/users/{userId:guid}/sessions/{sessionId:guid}", Handler)
        .WithName("AdminRevokeSession")
        .WithSummary("Revoke a user's session (Admin)")
        .RequirePermission(IdentityPermissionConstants.Sessions.RevokeAll)
        .WithDescription("Revoke a specific session for a user. Requires admin permission.");
    }

    private static async Task<IResult> Handler(
        Guid userId,
        Guid sessionId,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        bool result = await mediator.Send(new AdminRevokeSessionCommand(userId, sessionId), cancellationToken);
        return result ? Results.Ok() : Results.NotFound();
    }
}
