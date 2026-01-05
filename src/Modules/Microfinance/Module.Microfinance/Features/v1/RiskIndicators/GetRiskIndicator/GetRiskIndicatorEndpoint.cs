using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.RiskIndicators.GetRiskIndicator;
using FSH.Module.Microfinance.Contracts.v1.RiskIndicators;

namespace FSH.Module.Microfinance.Features.v1.RiskIndicators.GetRiskIndicator;

public static class GetRiskIndicatorEndpoint
{
    public static RouteHandlerBuilder MapGetRiskIndicatorEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetRiskIndicatorQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetRiskIndicatorEndpoint))
        .WithSummary("Get RiskIndicator")
        .Produces<RiskIndicatorDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.RiskIndicators.View);
    }
}
