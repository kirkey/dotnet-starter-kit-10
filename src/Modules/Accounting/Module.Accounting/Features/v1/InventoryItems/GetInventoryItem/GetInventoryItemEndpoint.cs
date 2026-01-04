using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.InventoryItems;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.InventoryItems.GetInventoryItem;

public static class GetInventoryItemEndpoint
{
    public static RouteHandlerBuilder MapGetInventoryItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetInventoryItemQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetInventoryItemEndpoint))
        .WithSummary("Get InventoryItem by ID")
        .Produces<InventoryItemDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.InventoryItems.View);
    }
}
