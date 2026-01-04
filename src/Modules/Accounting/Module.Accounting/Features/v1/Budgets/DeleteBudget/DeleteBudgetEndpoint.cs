using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Budgets.DeleteBudget;

public static class DeleteBudgetEndpoint
{
    public static RouteHandlerBuilder MapDeleteBudgetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteBudgetCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteBudgetEndpoint))
        .WithSummary("Delete Budget")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Budgets.Delete);
    }
}
