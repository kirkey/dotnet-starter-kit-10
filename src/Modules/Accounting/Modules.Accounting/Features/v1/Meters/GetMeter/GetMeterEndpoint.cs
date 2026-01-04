using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Accounting.Contracts.v1.Meters;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.Meters.GetMeter;

public static class GetMeterEndpoint
{
    public static RouteHandlerBuilder MapGetMeterEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetMeterQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetMeterEndpoint))
        .WithSummary("Get Meter by ID")
        .Produces<MeterDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Meters.View);
    }
}
