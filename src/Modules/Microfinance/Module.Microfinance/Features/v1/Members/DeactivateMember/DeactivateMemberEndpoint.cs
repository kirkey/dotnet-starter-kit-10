using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;


using FSH.Module.Microfinance.Contracts.v1.Members;

namespace FSH.Module.Microfinance.Features.v1.Members.DeactivateMember;

public static class DeactivateMemberEndpoint
{
    public static RouteHandlerBuilder MapDeactivateMemberEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/deactivate", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeactivateMemberCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(DeactivateMemberEndpoint))
        .WithSummary("Deactivate member")
        .Produces(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.Members.Deactivate);
    }
}
