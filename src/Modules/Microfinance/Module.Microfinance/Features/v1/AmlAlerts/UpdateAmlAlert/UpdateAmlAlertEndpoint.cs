using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Microfinance.Contracts.v1.AmlAlerts.UpdateAmlAlert;

namespace FSH.Module.Microfinance.Features.v1.AmlAlerts.UpdateAmlAlert;

public static class UpdateAmlAlertEndpoint
{
    public static RouteHandlerBuilder MapUpdateAmlAlertEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateAmlAlertCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateAmlAlertEndpoint))
        .WithSummary("Update AmlAlert")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.AmlAlerts.Update);
    }
}
