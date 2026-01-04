using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.ProjectCosts.DeleteProjectCost;

public static class DeleteProjectCostEndpoint
{
    public static RouteHandlerBuilder MapDeleteProjectCostEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteProjectCostCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteProjectCostEndpoint))
        .WithSummary("Delete ProjectCost")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.ProjectCosts.Delete);
    }
}
