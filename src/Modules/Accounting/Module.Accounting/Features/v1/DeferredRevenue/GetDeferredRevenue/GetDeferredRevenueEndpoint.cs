using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.DeferredRevenue;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.DeferredRevenue.GetDeferredRevenue;

namespace FSH.Module.Accounting.Features.v1.DeferredRevenue.GetDeferredRevenue;

public static class GetDeferredRevenueByIdEndpoint
{
    public static RouteHandlerBuilder MapGetDeferredRevenueByIdEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetDeferredRevenueByIdQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetDeferredRevenueByIdEndpoint))
        .WithSummary("Get DeferredRevenue by ID")
        .Produces<DeferredRevenueDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.DeferredRevenue.View);
    }
}
