using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.Projects.DeleteProject;

public static class DeleteProjectEndpoint
{
    public static RouteHandlerBuilder MapDeleteProjectEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteProjectCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteProjectEndpoint))
        .WithSummary("Delete Project")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Projects.Delete);
    }
}
