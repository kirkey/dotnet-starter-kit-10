using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.QrPayments;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.QrPayments.GetQrPayment;

public static class GetQrPaymentEndpoint
{
    public static RouteHandlerBuilder MapGetQrPaymentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetQrPaymentQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetQrPaymentEndpoint))
        .WithSummary("Get QrPayment")
        .Produces<QrPaymentDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.QrPayments.View);
    }
}
