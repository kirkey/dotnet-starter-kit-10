using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.ProjectCosts.CreateProjectCost;

public static class CreateProjectCostEndpoint
{
    public static RouteHandlerBuilder MapCreateProjectCostEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateProjectCostCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting/projectcosts/{id}", id);
        })
        .WithName(nameof(CreateProjectCostEndpoint))
        .WithSummary("Create ProjectCost")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.ProjectCosts.Create);
    }
}
