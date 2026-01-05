using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.Payments.UpdatePayment;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Payments.UpdatePayment;

public static class UpdatePaymentEndpoint
{
    public static RouteHandlerBuilder MapUpdatePaymentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdatePaymentCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdatePaymentEndpoint))
        .WithSummary("Update Payment")
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Payments.Update);
    }
}
