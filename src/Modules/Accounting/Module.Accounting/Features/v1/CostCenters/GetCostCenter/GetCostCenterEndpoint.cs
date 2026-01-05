using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.CostCenters;
using FSH.Module.Accounting.Contracts.v1.CostCenters.GetCostCenter;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.CostCenters.GetCostCenter;

namespace FSH.Module.Accounting.Features.v1.CostCenters.GetCostCenter;

public static class GetCostCenterEndpoint
{
    public static RouteHandlerBuilder MapGetCostCenterEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCostCenterQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCostCenterEndpoint))
        .WithSummary("Get CostCenter by ID")
        .Produces<CostCenterDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.CostCenters.View);
    }
}
