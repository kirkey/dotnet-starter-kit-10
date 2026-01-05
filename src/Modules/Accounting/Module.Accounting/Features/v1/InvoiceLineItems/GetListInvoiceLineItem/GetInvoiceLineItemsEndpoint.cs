using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.InvoiceLineItems;
using FSH.Module.Accounting.Contracts.v1.InvoiceLineItems.GetListInvoiceLineItem;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.InvoiceLineItems.GetInvoiceLineItems;

public static class GetInvoiceLineItemsEndpoint
{
    public static RouteHandlerBuilder MapGetInvoiceLineItemsEndpoint(this IEndpointRouteBuilder endpoints)
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
                new GetInvoiceLineItemsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetInvoiceLineItemsEndpoint))
        .WithSummary("Get paginated list of InvoiceLineItems")
        .Produces<InvoiceLineItemsPagedResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.InvoiceLineItems.Search);
    }
}
