using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.InvoiceLineItems.DeleteInvoiceLineItem;

namespace FSH.Module.Accounting.Features.v1.InvoiceLineItems.DeleteInvoiceLineItem;

public static class DeleteInvoiceLineItemEndpoint
{
    public static RouteHandlerBuilder MapDeleteInvoiceLineItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteInvoiceLineItemCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteInvoiceLineItemEndpoint))
        .WithSummary("Delete InvoiceLineItem")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.InvoiceLineItems.Delete);
    }
}
