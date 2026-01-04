using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Payments.DeletePayment;

public static class DeletePaymentEndpoint
{
    public static RouteHandlerBuilder MapDeletePaymentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeletePaymentCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeletePaymentEndpoint))
        .WithSummary("Delete Payment")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Payments.Delete);
    }
}
