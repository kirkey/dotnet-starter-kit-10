// TODO: Implement Export endpoint for BankReconciliation
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.BankReconciliations.ExportBankReconciliation;

public static class ExportBankReconciliationEndpoint
{
    public static RouteHandlerBuilder MapExportBankReconciliationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ExportBankReconciliationCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ExportBankReconciliationEndpoint))
        .WithSummary("Export BankReconciliation")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.BankReconciliations.Export);
    }
}
