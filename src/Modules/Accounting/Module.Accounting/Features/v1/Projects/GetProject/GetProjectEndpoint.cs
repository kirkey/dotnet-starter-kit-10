using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.Projects;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Projects.GetProject;

public static class GetProjectEndpoint
{
    public static RouteHandlerBuilder MapGetProjectEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetProjectQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetProjectEndpoint))
        .WithSummary("Get Project by ID")
        .Produces<ProjectDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Projects.View);
    }
}
