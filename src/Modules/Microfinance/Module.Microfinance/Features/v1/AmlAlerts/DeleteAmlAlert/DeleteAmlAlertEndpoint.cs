using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Microfinance.Contracts.v1.AmlAlerts.DeleteAmlAlert;

namespace FSH.Module.Microfinance.Features.v1.AmlAlerts.DeleteAmlAlert;

public static class DeleteAmlAlertEndpoint
{
    public static RouteHandlerBuilder MapDeleteAmlAlertEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteAmlAlertCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteAmlAlertEndpoint))
        .WithSummary("Delete AmlAlert")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.AmlAlerts.Delete);
    }
}
