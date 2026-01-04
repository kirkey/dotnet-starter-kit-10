using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Accounting.Contracts.v1.DeferredRevenue;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.DeferredRevenue.GetDeferredRevenue;

public static class GetDeferredRevenueEndpoint
{
    public static RouteHandlerBuilder MapGetDeferredRevenueEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetDeferredRevenueQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetDeferredRevenueEndpoint))
        .WithSummary("Get DeferredRevenue by ID")
        .Produces<DeferredRevenueDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.DeferredRevenue.View);
    }
}
