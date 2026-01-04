using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.RiskIndicators.DeleteRiskIndicator;

public static class DeleteRiskIndicatorEndpoint
{
    public static RouteHandlerBuilder MapDeleteRiskIndicatorEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteRiskIndicatorCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteRiskIndicatorEndpoint))
        .WithSummary("Delete RiskIndicator")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.RiskIndicators.Delete);
    }
}
