// TODO: Implement Approve endpoint for AccountReconciliation
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.AccountReconciliations.ApproveAccountReconciliation;

namespace FSH.Module.Accounting.Features.v1.AccountReconciliations.ApproveAccountReconciliation;

public static class ApproveAccountReconciliationEndpoint
{
    public static RouteHandlerBuilder MapApproveAccountReconciliationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ApproveAccountReconciliationCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ApproveAccountReconciliationEndpoint))
        .WithSummary("Approve AccountReconciliation")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.AccountReconciliations.Approve);
    }
}
