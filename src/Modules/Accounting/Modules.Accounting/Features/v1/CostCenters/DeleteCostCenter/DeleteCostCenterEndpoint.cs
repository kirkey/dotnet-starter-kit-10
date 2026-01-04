using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.CostCenters.DeleteCostCenter;

public static class DeleteCostCenterEndpoint
{
    public static RouteHandlerBuilder MapDeleteCostCenterEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteCostCenterCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteCostCenterEndpoint))
        .WithSummary("Delete CostCenter")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.CostCenters.Delete);
    }
}
