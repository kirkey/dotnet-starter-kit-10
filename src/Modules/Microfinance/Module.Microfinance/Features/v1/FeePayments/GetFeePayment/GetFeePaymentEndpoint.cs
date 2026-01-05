using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.FeePayments.GetFeePayment;
using FSH.Module.Microfinance.Contracts.v1.FeePayments;

namespace FSH.Module.Microfinance.Features.v1.FeePayments.GetFeePayment;

public static class GetFeePaymentEndpoint
{
    public static RouteHandlerBuilder MapGetFeePaymentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetFeePaymentQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetFeePaymentEndpoint))
        .WithSummary("Get FeePayment")
        .Produces<FeePaymentDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.FeePayments.View);
    }
}
