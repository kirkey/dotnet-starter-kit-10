using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.CollateralValuations.DeleteCollateralValuation;

public static class DeleteCollateralValuationEndpoint
{
    public static RouteHandlerBuilder MapDeleteCollateralValuationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteCollateralValuationCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteCollateralValuationEndpoint))
        .WithSummary("Delete CollateralValuation")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.CollateralValuations.Delete);
    }
}
