using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.RiskAlerts.UpdateRiskAlert;

namespace FSH.Module.Microfinance.Features.v1.RiskAlerts.UpdateRiskAlert;

public static class UpdateRiskAlertEndpoint
{
    public static RouteHandlerBuilder MapUpdateRiskAlertEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateRiskAlertCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateRiskAlertEndpoint))
        .WithSummary("Update RiskAlert")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.RiskAlerts.Update);
    }
}
