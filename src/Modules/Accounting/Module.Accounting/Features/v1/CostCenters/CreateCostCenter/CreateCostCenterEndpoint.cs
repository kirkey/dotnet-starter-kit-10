using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.CostCenters.CreateCostCenter;

public static class CreateCostCenterEndpoint
{
    public static RouteHandlerBuilder MapCreateCostCenterEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateCostCenterCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting/costcenters/{id}", id);
        })
        .WithName(nameof(CreateCostCenterEndpoint))
        .WithSummary("Create CostCenter")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.CostCenters.Create);
    }
}
