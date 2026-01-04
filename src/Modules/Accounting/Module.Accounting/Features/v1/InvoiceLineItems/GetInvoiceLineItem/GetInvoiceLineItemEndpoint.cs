using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.InvoiceLineItems;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.InvoiceLineItems.GetInvoiceLineItem;

public static class GetInvoiceLineItemEndpoint
{
    public static RouteHandlerBuilder MapGetInvoiceLineItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetInvoiceLineItemQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetInvoiceLineItemEndpoint))
        .WithSummary("Get InvoiceLineItem by ID")
        .Produces<InvoiceLineItemDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.InvoiceLineItems.View);
    }
}
