using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.CollateralInsurances;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.CollateralInsurances.GetCollateralInsurance;

public static class GetCollateralInsuranceEndpoint
{
    public static RouteHandlerBuilder MapGetCollateralInsuranceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCollateralInsuranceQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCollateralInsuranceEndpoint))
        .WithSummary("Get CollateralInsurance")
        .Produces<CollateralInsuranceDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CollateralInsurances.View);
    }
}
