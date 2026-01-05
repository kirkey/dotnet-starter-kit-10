using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.ReportGenerations.UpdateReportGeneration;

namespace FSH.Module.Microfinance.Features.v1.ReportGenerations.UpdateReportGeneration;

public static class UpdateReportGenerationEndpoint
{
    public static RouteHandlerBuilder MapUpdateReportGenerationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateReportGenerationCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateReportGenerationEndpoint))
        .WithSummary("Update ReportGeneration")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.ReportGenerations.Update);
    }
}
