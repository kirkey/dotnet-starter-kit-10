using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.RateSchedules.DeleteRateSchedule;

public static class DeleteRateScheduleEndpoint
{
    public static RouteHandlerBuilder MapDeleteRateScheduleEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteRateScheduleCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteRateScheduleEndpoint))
        .WithSummary("Delete RateSchedule")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.RateSchedules.Delete);
    }
}
