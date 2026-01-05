using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.BillLineItems;
using FSH.Module.Accounting.Contracts.v1.BillLineItems.GetBillLineItem;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.BillLineItems.GetBillLineItem;

public static class GetBillLineItemEndpoint
{
    public static RouteHandlerBuilder MapGetBillLineItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetBillLineItemQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetBillLineItemEndpoint))
        .WithSummary("Get BillLineItem by ID")
        .Produces<BillLineItemDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.BillLineItems.View);
    }
}
