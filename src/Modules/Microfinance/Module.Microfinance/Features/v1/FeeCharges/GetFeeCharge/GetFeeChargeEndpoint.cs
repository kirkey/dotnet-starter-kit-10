using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.FeeCharges.GetFeeCharge;
using FSH.Module.Microfinance.Contracts.v1.FeeCharges;

namespace FSH.Module.Microfinance.Features.v1.FeeCharges.GetFeeCharge;

public static class GetFeeChargeEndpoint
{
    public static RouteHandlerBuilder MapGetFeeChargeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetFeeChargeQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetFeeChargeEndpoint))
        .WithSummary("Get FeeCharge")
        .Produces<FeeChargeDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.FeeCharges.View);
    }
}
