using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CollateralReleases.GetCollateralRelease;
using FSH.Module.Microfinance.Contracts.v1.CollateralReleases;

namespace FSH.Module.Microfinance.Features.v1.CollateralReleases.GetCollateralRelease;

public static class GetCollateralReleaseEndpoint
{
    public static RouteHandlerBuilder MapGetCollateralReleaseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCollateralReleaseQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCollateralReleaseEndpoint))
        .WithSummary("Get CollateralRelease")
        .Produces<CollateralReleaseDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CollateralReleases.View);
    }
}
