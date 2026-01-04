using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.FeeWaivers.DeleteFeeWaiver;

public static class DeleteFeeWaiverEndpoint
{
    public static RouteHandlerBuilder MapDeleteFeeWaiverEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteFeeWaiverCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteFeeWaiverEndpoint))
        .WithSummary("Delete FeeWaiver")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.FeeWaivers.Delete);
    }
}
