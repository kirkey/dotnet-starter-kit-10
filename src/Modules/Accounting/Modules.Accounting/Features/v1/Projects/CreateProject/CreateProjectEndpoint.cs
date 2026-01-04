using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.Projects.CreateProject;

public static class CreateProjectEndpoint
{
    public static RouteHandlerBuilder MapCreateProjectEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateProjectCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting/projects/{id}", id);
        })
        .WithName(nameof(CreateProjectEndpoint))
        .WithSummary("Create Project")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Projects.Create);
    }
}
