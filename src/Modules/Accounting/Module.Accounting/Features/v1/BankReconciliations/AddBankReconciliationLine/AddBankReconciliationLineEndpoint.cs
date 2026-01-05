using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.BankReconciliations;
using FSH.Module.Accounting.Contracts.v1.BankReconciliations.AddBankReconciliationLine;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.BankReconciliations.AddBankReconciliationLine;

public static class AddBankReconciliationLineEndpoint
{
    public static RouteHandlerBuilder MapAddBankReconciliationLineEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/lines", async (
            Guid id,
            AddBankReconciliationLineCommand request,
            IMediator mediator,
            CancellationToken ct) =>
        {
            // Ensure route and body have consistent ReconciliationId
            var cmd = request with { BankReconciliationId = id };
            var newId = await mediator.Send(cmd, ct);
            return Results.Created($"/bankreconciliations/{id}/lines/{newId}", newId);
        })
        .WithName(nameof(AddBankReconciliationLineEndpoint))
        .WithSummary("Add a line to a bank reconciliation")
        .Produces(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.BankReconciliations.Create);
    }
}