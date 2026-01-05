// TODO: Implement Reconcile endpoint for InterCompanyTransaction
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.InterCompanyTransactions.ReconcileInterCompanyTransaction;

namespace FSH.Module.Accounting.Features.v1.InterCompanyTransactions.ReconcileInterCompanyTransaction;

public static class ReconcileInterCompanyTransactionEndpoint
{
    public static RouteHandlerBuilder MapReconcileInterCompanyTransactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ReconcileInterCompanyTransactionCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ReconcileInterCompanyTransactionEndpoint))
        .WithSummary("Reconcile InterCompanyTransaction")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.InterCompanyTransactions.Reconcile);
    }
}
