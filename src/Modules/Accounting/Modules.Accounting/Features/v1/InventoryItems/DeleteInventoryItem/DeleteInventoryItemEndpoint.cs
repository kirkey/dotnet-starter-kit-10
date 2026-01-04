using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.InventoryItems.DeleteInventoryItem;

public static class DeleteInventoryItemEndpoint
{
    public static RouteHandlerBuilder MapDeleteInventoryItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteInventoryItemCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteInventoryItemEndpoint))
        .WithSummary("Delete InventoryItem")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.InventoryItems.Delete);
    }
}
