using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.GroupMemberships;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.GroupMemberships.GetGroupMembership;

public static class GetGroupMembershipEndpoint
{
    public static RouteHandlerBuilder MapGetGroupMembershipEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetGroupMembershipQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetGroupMembershipEndpoint))
        .WithSummary("Get GroupMembership")
        .Produces<GroupMembershipDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.GroupMemberships.View);
    }
}
