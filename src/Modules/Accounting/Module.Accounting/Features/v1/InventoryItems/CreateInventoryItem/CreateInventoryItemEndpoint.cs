using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.InventoryItems.CreateInventoryItem;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.InventoryItems.CreateInventoryItem;

public static class CreateInventoryItemEndpoint
{
    public static RouteHandlerBuilder MapCreateInventoryItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateInventoryItemCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting//{id}", id);
        })
        .WithName(nameof(CreateInventoryItemEndpoint))
        .WithSummary("Create InventoryItem")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.InventoryItems.Create);
    }
}
