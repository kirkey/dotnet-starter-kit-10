using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.ProjectCosts;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.ProjectCosts.GetListProjectCost;

namespace FSH.Module.Accounting.Features.v1.ProjectCosts.GetProjectCosts;

public static class GetProjectCostsEndpoint
{
    public static RouteHandlerBuilder MapGetProjectCostsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetProjectCostsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetProjectCostsEndpoint))
        .WithSummary("Get paginated list of ProjectCosts")
        .Produces<ProjectCostsPagedResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.ProjectCosts.Search);
    }
}
