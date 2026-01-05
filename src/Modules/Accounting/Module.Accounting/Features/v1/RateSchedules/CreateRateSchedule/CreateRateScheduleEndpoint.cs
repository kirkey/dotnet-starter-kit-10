using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.RateSchedules.CreateRateSchedule;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing; 

namespace FSH.Module.Accounting.Features.v1.RateSchedules.CreateRateSchedule;

public static class CreateRateScheduleEndpoint
{
    public static RouteHandlerBuilder MapCreateRateScheduleEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateRateScheduleCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting/rateschedules/{id}", id);
        })
        .WithName(nameof(CreateRateScheduleEndpoint))
        .WithSummary("Create RateSchedule")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.RateSchedules.Create);
    }
}
