using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.RateSchedules;
using FSH.Module.Accounting.Contracts.v1.RateSchedules.GetRateSchedule;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.RateSchedules.GetRateSchedule;

public static class GetRateScheduleEndpoint
{
    public static RouteHandlerBuilder MapGetRateScheduleEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetRateScheduleQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetRateScheduleEndpoint))
        .WithSummary("Get RateSchedule by ID")
        .Produces<RateScheduleDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.RateSchedules.View);
    }
}
