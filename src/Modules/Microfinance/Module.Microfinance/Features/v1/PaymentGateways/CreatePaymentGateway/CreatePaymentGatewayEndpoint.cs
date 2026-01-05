using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.PaymentGateways.CreatePaymentGateway;

namespace FSH.Module.Microfinance.Features.v1.PaymentGateways.CreatePaymentGateway;

public static class CreatePaymentGatewayEndpoint
{
    public static RouteHandlerBuilder MapCreatePaymentGatewayEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreatePaymentGatewayCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreatePaymentGatewayEndpoint))
        .WithSummary("Create PaymentGateway")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.PaymentGateways.Create);
    }
}
