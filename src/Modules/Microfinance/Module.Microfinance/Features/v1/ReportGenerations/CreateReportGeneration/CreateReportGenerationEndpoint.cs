using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.ReportGenerations.CreateReportGeneration;

namespace FSH.Module.Microfinance.Features.v1.ReportGenerations.CreateReportGeneration;

public static class CreateReportGenerationEndpoint
{
    public static RouteHandlerBuilder MapCreateReportGenerationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateReportGenerationCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateReportGenerationEndpoint))
        .WithSummary("Create ReportGeneration")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.ReportGenerations.Create);
    }
}
