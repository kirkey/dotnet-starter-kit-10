using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.BudgetDetails.UpdateBudgetDetail;

namespace FSH.Module.Accounting.Features.v1.BudgetDetails.UpdateBudgetDetail;

public static class UpdateBudgetDetailEndpoint
{
    public static RouteHandlerBuilder MapUpdateBudgetDetailEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateBudgetDetailCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateBudgetDetailEndpoint))
        .WithSummary("Update BudgetDetail")
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.BudgetDetails.Update);
    }
}
