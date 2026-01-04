using FSH.Framework.Shared.Identity;
using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Identity.Contracts.v1.Roles.UpdatePermissions;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Identity.Features.v1.Roles.UpdateRolePermissions;

public static class UpdateRolePermissionsEndpoint
{
    public static RouteHandlerBuilder MapUpdateRolePermissionsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id}/permissions", async (
            string id,
            [FromBody] UpdatePermissionsCommand request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            if (id != request.RoleId)
            {
                return Results.BadRequest();
            }

            string response = await mediator.Send(request, cancellationToken);
            return Results.Ok(response);
        })
        .WithName("UpdateRolePermissions")
        .WithSummary("Update role permissions")
        .RequirePermission(IdentityPermissionConstants.Roles.Update)
        .WithDescription("Replace the set of permissions assigned to a role.");
    }
}
