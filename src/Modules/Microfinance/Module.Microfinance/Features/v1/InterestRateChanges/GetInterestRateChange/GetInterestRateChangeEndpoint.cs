using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.InterestRateChanges.GetInterestRateChange;
using FSH.Module.Microfinance.Contracts.v1.InterestRateChanges;

namespace FSH.Module.Microfinance.Features.v1.InterestRateChanges.GetInterestRateChange;

public static class GetInterestRateChangeEndpoint
{
    public static RouteHandlerBuilder MapGetInterestRateChangeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetInterestRateChangeQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetInterestRateChangeEndpoint))
        .WithSummary("Get InterestRateChange")
        .Produces<InterestRateChangeDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.InterestRateChanges.View);
    }
}
