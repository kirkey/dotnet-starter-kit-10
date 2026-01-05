using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.InventoryItems.AddStockInventoryItem;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.InventoryItems.AddStockInventoryItem;

namespace FSH.Module.Accounting.Features.v1.InventoryItems.AddStockInventoryItem;

public static class AddStockInventoryItemEndpoint
{
    public static RouteHandlerBuilder MapAddStockInventoryItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/add-stock", async (
            Guid id,
            AddStockRequest request,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new AddStockInventoryItemCommand(id, request.Quantity), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(AddStockInventoryItemEndpoint))
        .WithSummary("AddStock InventoryItem")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.InventoryItems.AddStock);
    }
}

public record AddStockRequest(decimal Quantity);
