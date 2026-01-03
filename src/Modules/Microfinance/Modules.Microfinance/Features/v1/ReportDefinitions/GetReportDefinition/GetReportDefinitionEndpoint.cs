using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.ReportDefinitions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.ReportDefinitions.GetReportDefinition;

public static class GetReportDefinitionEndpoint
{
    public static RouteHandlerBuilder MapGetReportDefinitionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetReportDefinitionQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetReportDefinitionEndpoint))
        .WithSummary("Get ReportDefinition")
        .Produces<ReportDefinitionDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.ReportDefinitions.View);
    }
}
