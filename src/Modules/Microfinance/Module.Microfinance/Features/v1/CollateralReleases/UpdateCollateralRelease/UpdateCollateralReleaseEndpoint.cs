using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CollateralReleases.UpdateCollateralRelease;

namespace FSH.Module.Microfinance.Features.v1.CollateralReleases.UpdateCollateralRelease;

public static class UpdateCollateralReleaseEndpoint
{
    public static RouteHandlerBuilder MapUpdateCollateralReleaseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateCollateralReleaseCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateCollateralReleaseEndpoint))
        .WithSummary("Update CollateralRelease")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CollateralReleases.Update);
    }
}
