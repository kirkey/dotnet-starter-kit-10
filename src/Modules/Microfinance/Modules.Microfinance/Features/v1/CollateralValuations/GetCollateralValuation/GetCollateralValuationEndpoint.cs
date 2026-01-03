using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.CollateralValuations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.CollateralValuations.GetCollateralValuation;

public static class GetCollateralValuationEndpoint
{
    public static RouteHandlerBuilder MapGetCollateralValuationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCollateralValuationQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCollateralValuationEndpoint))
        .WithSummary("Get CollateralValuation")
        .Produces<CollateralValuationDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CollateralValuations.View);
    }
}
