using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Microfinance.Contracts.v1.AmlAlerts.GetAmlAlert;
using FSH.Module.Microfinance.Contracts.v1.AmlAlerts;

namespace FSH.Module.Microfinance.Features.v1.AmlAlerts.GetAmlAlert;

public static class GetAmlAlertEndpoint
{
    public static RouteHandlerBuilder MapGetAmlAlertEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAmlAlertQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetAmlAlertEndpoint))
        .WithSummary("Get AmlAlert")
        .Produces<AmlAlertDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.AmlAlerts.View);
    }
}
