using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.ReportGenerations.DeleteReportGeneration;

public static class DeleteReportGenerationEndpoint
{
    public static RouteHandlerBuilder MapDeleteReportGenerationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteReportGenerationCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteReportGenerationEndpoint))
        .WithSummary("Delete ReportGeneration")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.ReportGenerations.Delete);
    }
}
