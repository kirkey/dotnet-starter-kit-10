// TODO: Implement Approve endpoint for Payment
using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.Payments.ApprovePayment;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Payments.ApprovePayment;

public static class ApprovePaymentEndpoint
{
    public static RouteHandlerBuilder MapApprovePaymentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ApprovePaymentCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ApprovePaymentEndpoint))
        .WithSummary("Approve Payment")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Payments.Approve);
    }
}
