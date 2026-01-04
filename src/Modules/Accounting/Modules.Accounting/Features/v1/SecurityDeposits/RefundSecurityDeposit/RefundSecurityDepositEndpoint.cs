// TODO: Implement Refund endpoint for SecurityDeposit
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.SecurityDeposits.RefundSecurityDeposit;

public static class RefundSecurityDepositEndpoint
{
    public static RouteHandlerBuilder MapRefundSecurityDepositEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new RefundSecurityDepositCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(RefundSecurityDepositEndpoint))
        .WithSummary("Refund SecurityDeposit")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.SecurityDeposits.Refund);
    }
}
