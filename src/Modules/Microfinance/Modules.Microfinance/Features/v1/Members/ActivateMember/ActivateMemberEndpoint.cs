using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.Members.ActivateMember;

public static class ActivateMemberEndpoint
{
    public static RouteHandlerBuilder MapActivateMemberEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/activate", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ActivateMemberCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ActivateMemberEndpoint))
        .WithSummary("Activate member")
        .Produces(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.Members.Activate);
    }
}
