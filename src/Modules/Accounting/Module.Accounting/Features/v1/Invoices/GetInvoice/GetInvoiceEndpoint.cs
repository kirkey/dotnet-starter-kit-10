using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.Invoices;
using FSH.Module.Accounting.Contracts.v1.Invoices.GetInvoice;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Invoices.GetInvoice;

public static class GetInvoiceEndpoint
{
    public static RouteHandlerBuilder MapGetInvoiceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetInvoiceQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetInvoiceEndpoint))
        .WithSummary("Get Invoice by ID")
        .Produces<InvoiceDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Invoices.View);
    }
}
