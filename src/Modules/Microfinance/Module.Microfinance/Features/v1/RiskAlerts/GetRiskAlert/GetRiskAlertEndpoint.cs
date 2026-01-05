using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.RiskAlerts.GetRiskAlert;
using FSH.Module.Microfinance.Contracts.v1.RiskAlerts;

namespace FSH.Module.Microfinance.Features.v1.RiskAlerts.GetRiskAlert;

public static class GetRiskAlertEndpoint
{
    public static RouteHandlerBuilder MapGetRiskAlertEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetRiskAlertQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetRiskAlertEndpoint))
        .WithSummary("Get RiskAlert")
        .Produces<RiskAlertDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.RiskAlerts.View);
    }
}
