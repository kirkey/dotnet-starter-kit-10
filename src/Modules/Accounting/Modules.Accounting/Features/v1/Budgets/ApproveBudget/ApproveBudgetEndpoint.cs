// TODO: Implement Approve endpoint for Budget
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.Budgets.ApproveBudget;

public static class ApproveBudgetEndpoint
{
    public static RouteHandlerBuilder MapApproveBudgetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ApproveBudgetCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ApproveBudgetEndpoint))
        .WithSummary("Approve Budget")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Budgets.Approve);
    }
}
