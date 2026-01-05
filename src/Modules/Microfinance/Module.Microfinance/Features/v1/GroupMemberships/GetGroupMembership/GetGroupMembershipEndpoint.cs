using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.GroupMemberships.GetGroupMembership;
using FSH.Module.Microfinance.Contracts.v1.GroupMemberships;

namespace FSH.Module.Microfinance.Features.v1.GroupMemberships.GetGroupMembership;

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
