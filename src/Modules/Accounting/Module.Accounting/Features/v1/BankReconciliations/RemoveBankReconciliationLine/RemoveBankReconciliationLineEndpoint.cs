using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.BankReconciliations.RemoveBankReconciliationLine;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.BankReconciliations.RemoveBankReconciliationLine;

public static class RemoveBankReconciliationLineEndpoint
{
    public static RouteHandlerBuilder MapRemoveBankReconciliationLineEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}/lines/{lineId:guid}", async (
            Guid id,
            Guid lineId,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new RemoveBankReconciliationLineCommand(lineId), ct);
            return Results.NoContent();
        })
        .WithName(nameof(RemoveBankReconciliationLineEndpoint))
        .WithSummary("Remove a line from a bank reconciliation")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.BankReconciliations.Approve);
    }
}