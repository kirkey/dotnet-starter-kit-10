using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.PaymentGateways.DeletePaymentGateway;

public static class DeletePaymentGatewayEndpoint
{
    public static RouteHandlerBuilder MapDeletePaymentGatewayEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeletePaymentGatewayCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeletePaymentGatewayEndpoint))
        .WithSummary("Delete PaymentGateway")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.PaymentGateways.Delete);
    }
}
