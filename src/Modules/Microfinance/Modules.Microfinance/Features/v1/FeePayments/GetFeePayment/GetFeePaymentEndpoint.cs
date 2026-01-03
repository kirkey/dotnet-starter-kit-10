using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.FeePayments;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.FeePayments.GetFeePayment;

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
