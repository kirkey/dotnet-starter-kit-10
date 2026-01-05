using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CollateralReleases.GetCollateralReleases;
using FSH.Module.Microfinance.Contracts.v1.CollateralReleases;

namespace FSH.Module.Microfinance.Features.v1.CollateralReleases.GetCollateralReleases;

public static class GetCollateralReleasesEndpoint
{
    public static RouteHandlerBuilder MapGetCollateralReleasesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCollateralReleasesQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCollateralReleasesEndpoint))
        .WithSummary("Get CollateralReleases")
        .Produces<CollateralReleasesPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CollateralReleases.Search);
    }
}
