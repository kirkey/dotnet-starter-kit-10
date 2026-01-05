using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.FeePayments.UpdateFeePayment;

namespace FSH.Module.Microfinance.Features.v1.FeePayments.UpdateFeePayment;

public static class UpdateFeePaymentEndpoint
{
    public static RouteHandlerBuilder MapUpdateFeePaymentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateFeePaymentCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateFeePaymentEndpoint))
        .WithSummary("Update FeePayment")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.FeePayments.Update);
    }
}
