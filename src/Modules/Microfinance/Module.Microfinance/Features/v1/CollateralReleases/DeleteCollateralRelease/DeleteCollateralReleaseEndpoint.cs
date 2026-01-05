using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CollateralReleases.DeleteCollateralRelease;

namespace FSH.Module.Microfinance.Features.v1.CollateralReleases.DeleteCollateralRelease;

public static class DeleteCollateralReleaseEndpoint
{
    public static RouteHandlerBuilder MapDeleteCollateralReleaseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteCollateralReleaseCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteCollateralReleaseEndpoint))
        .WithSummary("Delete CollateralRelease")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.CollateralReleases.Delete);
    }
}
