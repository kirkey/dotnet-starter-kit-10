using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.InventoryItems.ReduceStockInventoryItem;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.InventoryItems.ReduceStockInventoryItem;

namespace FSH.Module.Accounting.Features.v1.InventoryItems.ReduceStockInventoryItem;

public static class ReduceStockInventoryItemEndpoint
{
    public static RouteHandlerBuilder MapReduceStockInventoryItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/reduce-stock", async (
            Guid id,
            ReduceStockRequest request,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ReduceStockInventoryItemCommand(id, request.Quantity), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ReduceStockInventoryItemEndpoint))
        .WithSummary("ReduceStock InventoryItem")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.InventoryItems.ReduceStock);
    }
}

public record ReduceStockRequest(decimal Quantity);
