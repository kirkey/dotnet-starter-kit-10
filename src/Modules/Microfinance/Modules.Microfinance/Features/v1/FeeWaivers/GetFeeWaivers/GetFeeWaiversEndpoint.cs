using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.FeeWaivers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.FeeWaivers.GetFeeWaivers;

public static class GetFeeWaiversEndpoint
{
    public static RouteHandlerBuilder MapGetFeeWaiversEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetFeeWaiversQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetFeeWaiversEndpoint))
        .WithSummary("Get FeeWaivers")
        .Produces<FeeWaiversPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.FeeWaivers.Search);
    }
}
