using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.QrPayments.CreateQrPayment;

namespace FSH.Module.Microfinance.Features.v1.QrPayments.CreateQrPayment;

public static class CreateQrPaymentEndpoint
{
    public static RouteHandlerBuilder MapCreateQrPaymentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateQrPaymentCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateQrPaymentEndpoint))
        .WithSummary("Create QrPayment")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.QrPayments.Create);
    }
}
