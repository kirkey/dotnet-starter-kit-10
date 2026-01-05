using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.GroupMemberships.UpdateGroupMembership;

namespace FSH.Module.Microfinance.Features.v1.GroupMemberships.UpdateGroupMembership;

public static class UpdateGroupMembershipEndpoint
{
    public static RouteHandlerBuilder MapUpdateGroupMembershipEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateGroupMembershipCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateGroupMembershipEndpoint))
        .WithSummary("Update GroupMembership")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.GroupMemberships.Update);
    }
}
