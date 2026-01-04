using FSH.Framework.Shared.Identity;
using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Identity.Contracts.v1.Sessions.RevokeSession;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Identity.Features.v1.Sessions.RevokeSession;

public static class RevokeSessionEndpoint
{
    internal static RouteHandlerBuilder MapRevokeSessionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/sessions/{sessionId:guid}", Handler)
        .WithName("RevokeSession")
        .WithSummary("Revoke a session")
        .RequirePermission(IdentityPermissionConstants.Sessions.Revoke)
        .WithDescription("Revoke a specific session for the currently authenticated user.");
    }

    private static async Task<IResult> Handler(
        Guid sessionId,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        bool result = await mediator.Send(new RevokeSessionCommand(sessionId), cancellationToken);
        return result ? Results.Ok() : Results.NotFound();
    }
}
