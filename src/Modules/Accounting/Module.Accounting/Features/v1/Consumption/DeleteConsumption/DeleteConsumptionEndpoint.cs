using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Consumption.DeleteConsumption;

public static class DeleteConsumptionEndpoint
{
    public static RouteHandlerBuilder MapDeleteConsumptionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteConsumptionCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteConsumptionEndpoint))
        .WithSummary("Delete Consumption")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Consumption.Delete);
    }
}
