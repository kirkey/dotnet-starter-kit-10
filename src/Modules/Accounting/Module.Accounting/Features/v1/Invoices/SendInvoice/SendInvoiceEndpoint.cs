// TODO: Implement Send endpoint for Invoice
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Invoices.SendInvoice;

public static class SendInvoiceEndpoint
{
    public static RouteHandlerBuilder MapSendInvoiceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new SendInvoiceCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(SendInvoiceEndpoint))
        .WithSummary("Send Invoice")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Invoices.Send);
    }
}
