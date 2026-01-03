using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.PaymentGateways;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.PaymentGateways.GetPaymentGateway;

public static class GetPaymentGatewayEndpoint
{
    public static RouteHandlerBuilder MapGetPaymentGatewayEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPaymentGatewayQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetPaymentGatewayEndpoint))
        .WithSummary("Get PaymentGateway")
        .Produces<PaymentGatewayDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.PaymentGateways.View);
    }
}
