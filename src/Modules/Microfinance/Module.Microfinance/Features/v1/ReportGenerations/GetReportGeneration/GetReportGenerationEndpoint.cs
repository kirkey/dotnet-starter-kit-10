using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.ReportGenerations.GetReportGeneration;
using FSH.Module.Microfinance.Contracts.v1.ReportGenerations;

namespace FSH.Module.Microfinance.Features.v1.ReportGenerations.GetReportGeneration;

public static class GetReportGenerationEndpoint
{
    public static RouteHandlerBuilder MapGetReportGenerationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetReportGenerationQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetReportGenerationEndpoint))
        .WithSummary("Get ReportGeneration")
        .Produces<ReportGenerationDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.ReportGenerations.View);
    }
}
