using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.Meters.DeleteMeter;

public static class DeleteMeterEndpoint
{
    public static RouteHandlerBuilder MapDeleteMeterEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteMeterCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteMeterEndpoint))
        .WithSummary("Delete Meter")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Meters.Delete);
    }
}
