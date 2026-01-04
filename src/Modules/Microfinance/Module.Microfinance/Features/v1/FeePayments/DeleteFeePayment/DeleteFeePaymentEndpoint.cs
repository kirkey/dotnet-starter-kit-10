using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.FeePayments.DeleteFeePayment;

public static class DeleteFeePaymentEndpoint
{
    public static RouteHandlerBuilder MapDeleteFeePaymentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteFeePaymentCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteFeePaymentEndpoint))
        .WithSummary("Delete FeePayment")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.FeePayments.Delete);
    }
}
