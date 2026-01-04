// TODO: Implement Submit endpoint for RegulatoryReport
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.RegulatoryReports.SubmitRegulatoryReport;

public static class SubmitRegulatoryReportEndpoint
{
    public static RouteHandlerBuilder MapSubmitRegulatoryReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new SubmitRegulatoryReportCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(SubmitRegulatoryReportEndpoint))
        .WithSummary("Submit RegulatoryReport")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.RegulatoryReports.Submit);
    }
}
