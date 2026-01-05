using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.InvoiceLineItems.CreateInvoiceLineItem;

namespace FSH.Module.Accounting.Features.v1.InvoiceLineItems.CreateInvoiceLineItem;

public static class CreateInvoiceLineItemEndpoint
{
    public static RouteHandlerBuilder MapCreateInvoiceLineItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateInvoiceLineItemCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting//{id}", id);
        })
        .WithName(nameof(CreateInvoiceLineItemEndpoint))
        .WithSummary("Create InvoiceLineItem")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.InvoiceLineItems.Create);
    }
}
