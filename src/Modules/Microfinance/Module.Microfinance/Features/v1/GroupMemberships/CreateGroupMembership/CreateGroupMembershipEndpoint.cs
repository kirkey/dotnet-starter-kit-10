using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.GroupMemberships.CreateGroupMembership;

namespace FSH.Module.Microfinance.Features.v1.GroupMemberships.CreateGroupMembership;

public static class CreateGroupMembershipEndpoint
{
    public static RouteHandlerBuilder MapCreateGroupMembershipEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateGroupMembershipCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateGroupMembershipEndpoint))
        .WithSummary("Create GroupMembership")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.GroupMemberships.Create);
    }
}
