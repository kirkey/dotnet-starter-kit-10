// TODO: Implement Export endpoint for RegulatoryReport
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.RegulatoryReports.ExportRegulatoryReport;

public static class ExportRegulatoryReportEndpoint
{
    public static RouteHandlerBuilder MapExportRegulatoryReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ExportRegulatoryReportCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ExportRegulatoryReportEndpoint))
        .WithSummary("Export RegulatoryReport")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.RegulatoryReports.Export);
    }
}
