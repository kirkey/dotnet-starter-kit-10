using FSH.Framework.Shared.Identity;
using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Identity.Contracts.v1.Users.GetUser;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Identity.Features.v1.Users.GetUserById;

public static class GetUserByIdEndpoint
{
    internal static RouteHandlerBuilder MapGetUserByIdEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/users/{id:guid}", (string id, IMediator mediator, CancellationToken cancellationToken) =>
            mediator.Send(new GetUserQuery(id), cancellationToken))
        .WithName("GetUser")
        .WithSummary("Get user by ID")
        .RequirePermission(IdentityPermissionConstants.Users.View)
        .WithDescription("Retrieve a user's profile details by unique user identifier.");
    }
}
