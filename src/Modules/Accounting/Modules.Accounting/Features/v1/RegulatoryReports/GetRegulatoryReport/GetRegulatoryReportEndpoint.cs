using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Accounting.Contracts.v1.RegulatoryReports;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.RegulatoryReports.GetRegulatoryReport;

public static class GetRegulatoryReportEndpoint
{
    public static RouteHandlerBuilder MapGetRegulatoryReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetRegulatoryReportQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetRegulatoryReportEndpoint))
        .WithSummary("Get RegulatoryReport by ID")
        .Produces<RegulatoryReportDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.RegulatoryReports.View);
    }
}
