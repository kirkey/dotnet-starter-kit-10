using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.PaymentAllocations.DeletePaymentAllocation;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.PaymentAllocations.DeletePaymentAllocation;

public static class DeletePaymentAllocationEndpoint
{
    public static RouteHandlerBuilder MapDeletePaymentAllocationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeletePaymentAllocationCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeletePaymentAllocationEndpoint))
        .WithSummary("Delete PaymentAllocation")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.PaymentAllocations.Delete);
    }
}
