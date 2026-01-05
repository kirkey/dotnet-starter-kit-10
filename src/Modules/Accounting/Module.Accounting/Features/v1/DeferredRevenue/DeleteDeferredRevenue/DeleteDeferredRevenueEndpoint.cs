using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.DeferredRevenue.DeleteDeferredRevenue;

namespace FSH.Module.Accounting.Features.v1.DeferredRevenue.DeleteDeferredRevenue;

public static class DeleteDeferredRevenueEndpoint
{
    public static RouteHandlerBuilder MapDeleteDeferredRevenueEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteDeferredRevenueCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteDeferredRevenueEndpoint))
        .WithSummary("Delete DeferredRevenue")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.DeferredRevenue.Delete);
    }
}
