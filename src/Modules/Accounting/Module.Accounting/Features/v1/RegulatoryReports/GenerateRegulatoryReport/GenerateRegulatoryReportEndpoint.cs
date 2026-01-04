// TODO: Implement Generate endpoint for RegulatoryReport
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.RegulatoryReports.GenerateRegulatoryReport;

public static class GenerateRegulatoryReportEndpoint
{
    public static RouteHandlerBuilder MapGenerateRegulatoryReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new GenerateRegulatoryReportCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(GenerateRegulatoryReportEndpoint))
        .WithSummary("Generate RegulatoryReport")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.RegulatoryReports.Generate);
    }
}
