using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.ProjectCosts.UpdateProjectCost;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.ProjectCosts.UpdateProjectCost;

public static class UpdateProjectCostEndpoint
{
    public static RouteHandlerBuilder MapUpdateProjectCostEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateProjectCostCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateProjectCostEndpoint))
        .WithSummary("Update ProjectCost")
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.ProjectCosts.Update);
    }
}
