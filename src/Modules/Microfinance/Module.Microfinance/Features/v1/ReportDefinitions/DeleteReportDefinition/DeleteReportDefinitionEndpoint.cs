using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.ReportDefinitions.DeleteReportDefinition;

namespace FSH.Module.Microfinance.Features.v1.ReportDefinitions.DeleteReportDefinition;

public static class DeleteReportDefinitionEndpoint
{
    public static RouteHandlerBuilder MapDeleteReportDefinitionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteReportDefinitionCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteReportDefinitionEndpoint))
        .WithSummary("Delete ReportDefinition")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.ReportDefinitions.Delete);
    }
}
