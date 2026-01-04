using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Accounting.Contracts.v1.Budgets;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.Budgets.GetBudget;

public static class GetBudgetEndpoint
{
    public static RouteHandlerBuilder MapGetBudgetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetBudgetQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetBudgetEndpoint))
        .WithSummary("Get Budget by ID")
        .Produces<BudgetDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Budgets.View);
    }
}
