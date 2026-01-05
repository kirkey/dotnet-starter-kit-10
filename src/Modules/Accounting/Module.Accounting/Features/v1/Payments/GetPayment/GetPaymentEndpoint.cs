using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.Payments;
using FSH.Module.Accounting.Contracts.v1.Payments.GetPayment;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Payments.GetPayment;

public static class GetPaymentEndpoint
{
    public static RouteHandlerBuilder MapGetPaymentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPaymentQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetPaymentEndpoint))
        .WithSummary("Get Payment by ID")
        .Produces<PaymentDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Payments.View);
    }
}
