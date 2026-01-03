using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.FeeWaivers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.FeeWaivers.GetFeeWaiver;

public static class GetFeeWaiverEndpoint
{
    public static RouteHandlerBuilder MapGetFeeWaiverEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetFeeWaiverQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetFeeWaiverEndpoint))
        .WithSummary("Get FeeWaiver")
        .Produces<FeeWaiverDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.FeeWaivers.View);
    }
}
