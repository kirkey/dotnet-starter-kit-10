using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Invoices.DeleteInvoice;

public static class DeleteInvoiceEndpoint
{
    public static RouteHandlerBuilder MapDeleteInvoiceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteInvoiceCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteInvoiceEndpoint))
        .WithSummary("Delete Invoice")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Invoices.Delete);
    }
}
