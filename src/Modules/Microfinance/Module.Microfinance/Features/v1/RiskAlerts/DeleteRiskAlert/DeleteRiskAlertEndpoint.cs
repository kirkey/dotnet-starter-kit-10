using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.RiskAlerts.DeleteRiskAlert;

namespace FSH.Module.Microfinance.Features.v1.RiskAlerts.DeleteRiskAlert;

public static class DeleteRiskAlertEndpoint
{
    public static RouteHandlerBuilder MapDeleteRiskAlertEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteRiskAlertCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteRiskAlertEndpoint))
        .WithSummary("Delete RiskAlert")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.RiskAlerts.Delete);
    }
}
