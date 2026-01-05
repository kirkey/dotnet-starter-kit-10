using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.RateSchedules.UpdateRateSchedule;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing; 

namespace FSH.Module.Accounting.Features.v1.RateSchedules.UpdateRateSchedule;

public static class UpdateRateScheduleEndpoint
{
    public static RouteHandlerBuilder MapUpdateRateScheduleEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateRateScheduleCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateRateScheduleEndpoint))
        .WithSummary("Update RateSchedule")
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.RateSchedules.Update);
    }
}
