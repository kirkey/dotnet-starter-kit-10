using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.DeferredRevenue.UpdateDeferredRevenue;

namespace FSH.Module.Accounting.Features.v1.DeferredRevenue.UpdateDeferredRevenue;

public static class UpdateDeferredRevenueEndpoint
{
    public static RouteHandlerBuilder MapUpdateDeferredRevenueEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateDeferredRevenueCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateDeferredRevenueEndpoint))
        .WithSummary("Update DeferredRevenue")
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.DeferredRevenue.Update);
    }
}
