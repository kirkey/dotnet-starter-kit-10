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
        return endpoints.MapGet("/{id:guid}/export", async (
            Guid id,
            string format,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var query = new ExportGeneralLedgerQuery(id, format);
            var result = await mediator.Send(query, ct);
            return Results.File(result.Data, result.ContentType, result.FileName);
        })
        .WithName(nameof(ExportGeneralLedgerEndpoint))
        .WithSummary("Export GeneralLedger")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.GeneralLedger.Export);
    }
}
