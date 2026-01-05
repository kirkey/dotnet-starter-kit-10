using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.QrPayments.DeleteQrPayment;

namespace FSH.Module.Microfinance.Features.v1.QrPayments.DeleteQrPayment;

public static class DeleteQrPaymentEndpoint
{
    public static RouteHandlerBuilder MapDeleteQrPaymentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteQrPaymentCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteQrPaymentEndpoint))
        .WithSummary("Delete QrPayment")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.QrPayments.Delete);
    }
}
