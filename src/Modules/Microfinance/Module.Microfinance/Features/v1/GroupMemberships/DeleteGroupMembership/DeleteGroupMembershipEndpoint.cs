using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.GroupMemberships.DeleteGroupMembership;

namespace FSH.Module.Microfinance.Features.v1.GroupMemberships.DeleteGroupMembership;

public static class DeleteGroupMembershipEndpoint
{
    public static RouteHandlerBuilder MapDeleteGroupMembershipEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteGroupMembershipCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteGroupMembershipEndpoint))
        .WithSummary("Delete GroupMembership")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.GroupMemberships.Delete);
    }
}
