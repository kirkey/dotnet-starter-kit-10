using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.Projects.UpdateProject;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Projects.UpdateProject;

public static class UpdateProjectEndpoint
{
    public static RouteHandlerBuilder MapUpdateProjectEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateProjectCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateProjectEndpoint))
        .WithSummary("Update Project")
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Projects.Update);
    }
}
