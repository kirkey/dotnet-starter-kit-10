// TODO: Implement Approve endpoint for BankReconciliation
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.BankReconciliations.ApproveBankReconciliation;

public static class ApproveBankReconciliationEndpoint
{
    public static RouteHandlerBuilder MapApproveBankReconciliationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ApproveBankReconciliationCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ApproveBankReconciliationEndpoint))
        .WithSummary("Approve BankReconciliation")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.BankReconciliations.Approve);
    }
}
