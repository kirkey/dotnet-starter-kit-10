using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.InventoryItems;
using FSH.Module.Accounting.Contracts.v1.InventoryItems.GetListInventoryItem;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.InventoryItems.GetInventoryItems;

public static class GetInventoryItemsEndpoint
{
    public static RouteHandlerBuilder MapGetInventoryItemsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetInventoryItemsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetInventoryItemsEndpoint))
        .WithSummary("Get paginated list of InventoryItems")
        .Produces<InventoryItemsPagedResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.InventoryItems.Search);
    }
}
