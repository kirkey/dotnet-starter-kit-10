// TODO: Implement Approve endpoint for Invoice
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Invoices.ApproveInvoice;

public static class ApproveInvoiceEndpoint
{
    public static RouteHandlerBuilder MapApproveInvoiceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ApproveInvoiceCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ApproveInvoiceEndpoint))
        .WithSummary("Approve Invoice")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Invoices.Approve);
    }
}
