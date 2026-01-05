using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CollateralTypes.GetCollateralType;
using FSH.Module.Microfinance.Contracts.v1.CollateralTypes;

namespace FSH.Module.Microfinance.Features.v1.CollateralTypes.GetCollateralType;

public static class GetCollateralTypeEndpoint
{
    public static RouteHandlerBuilder MapGetCollateralTypeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCollateralTypeQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCollateralTypeEndpoint))
        .WithSummary("Get CollateralType")
        .Produces<CollateralTypeDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CollateralTypes.View);
    }
}
