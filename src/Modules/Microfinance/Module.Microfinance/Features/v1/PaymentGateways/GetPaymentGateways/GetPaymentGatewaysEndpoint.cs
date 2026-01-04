using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.PaymentGateways;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.PaymentGateways.GetPaymentGateways;

public static class GetPaymentGatewaysEndpoint
{
    public static RouteHandlerBuilder MapGetPaymentGatewaysEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPaymentGatewaysQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetPaymentGatewaysEndpoint))
        .WithSummary("Get PaymentGateways")
        .Produces<PaymentGatewaysPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.PaymentGateways.Search);
    }
}
