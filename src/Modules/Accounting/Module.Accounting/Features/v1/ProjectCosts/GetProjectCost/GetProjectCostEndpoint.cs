using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.ProjectCosts;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.ProjectCosts.GetProjectCost;

namespace FSH.Module.Accounting.Features.v1.ProjectCosts.GetProjectCost;

public static class GetProjectCostEndpoint
{
    public static RouteHandlerBuilder MapGetProjectCostEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetProjectCostQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetProjectCostEndpoint))
        .WithSummary("Get ProjectCost by ID")
        .Produces<ProjectCostDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.ProjectCosts.View);
    }
}
