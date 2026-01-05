// TODO: Implement Recognize endpoint for DeferredRevenue
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.DeferredRevenue.RecognizeDeferredRevenue;

namespace FSH.Module.Accounting.Features.v1.DeferredRevenue.RecognizeDeferredRevenue;

public static class RecognizeDeferredRevenueEndpoint
{
    public static RouteHandlerBuilder MapRecognizeDeferredRevenueEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new RecognizeDeferredRevenueCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(RecognizeDeferredRevenueEndpoint))
        .WithSummary("Recognize DeferredRevenue")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.DeferredRevenue.Recognize);
    }
}
