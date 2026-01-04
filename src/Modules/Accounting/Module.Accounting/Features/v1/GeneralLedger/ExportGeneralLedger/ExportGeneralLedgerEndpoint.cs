// TODO: Implement Export endpoint for GeneralLedger
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.GeneralLedger.ExportGeneralLedger;

public static class ExportGeneralLedgerEndpoint
{
    public static RouteHandlerBuilder MapExportGeneralLedgerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ExportGeneralLedgerCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ExportGeneralLedgerEndpoint))
        .WithSummary("Export GeneralLedger")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.GeneralLedger.Export);
    }
}
