using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.RiskAlerts.GetRiskAlerts;
using FSH.Module.Microfinance.Contracts.v1.RiskAlerts;

namespace FSH.Module.Microfinance.Features.v1.RiskAlerts.GetRiskAlerts;

public static class GetRiskAlertsEndpoint
{
    public static RouteHandlerBuilder MapGetRiskAlertsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetRiskAlertsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetRiskAlertsEndpoint))
        .WithSummary("Get RiskAlerts")
        .Produces<RiskAlertsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.RiskAlerts.Search);
    }
}
