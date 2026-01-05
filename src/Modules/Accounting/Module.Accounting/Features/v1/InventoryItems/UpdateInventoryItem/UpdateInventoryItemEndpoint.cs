using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.InventoryItems.UpdateInventoryItem;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.InventoryItems.UpdateInventoryItem;

namespace FSH.Module.Accounting.Features.v1.InventoryItems.UpdateInventoryItem;

public static class UpdateInventoryItemEndpoint
{
    public static RouteHandlerBuilder MapUpdateInventoryItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateInventoryItemCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateInventoryItemEndpoint))
        .WithSummary("Update InventoryItem")
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.InventoryItems.Update);
    }
}
