using FSH.Module.Identity.Contracts.v1.Users.ChangePassword;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Identity.Features.v1.Users.ChangePassword;

public static class ChangePasswordEndpoint
{
    internal static RouteHandlerBuilder MapChangePasswordEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/change-password", async (
            [FromBody] ChangePasswordCommand command,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            string result = await mediator.Send(command, cancellationToken);
            return Results.Ok(result);
        })
        .WithName("ChangePassword")
        .WithSummary("Change password")
        .WithDescription("Change the current user's password.");
    }
}
