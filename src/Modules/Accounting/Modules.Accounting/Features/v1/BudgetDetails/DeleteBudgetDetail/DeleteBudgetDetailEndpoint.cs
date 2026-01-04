using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.BudgetDetails.DeleteBudgetDetail;

public static class DeleteBudgetDetailEndpoint
{
    public static RouteHandlerBuilder MapDeleteBudgetDetailEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteBudgetDetailCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteBudgetDetailEndpoint))
        .WithSummary("Delete BudgetDetail")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.BudgetDetails.Delete);
    }
}
