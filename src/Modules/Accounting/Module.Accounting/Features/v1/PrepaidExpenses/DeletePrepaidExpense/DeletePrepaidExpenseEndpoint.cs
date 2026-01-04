using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.PrepaidExpenses.DeletePrepaidExpense;

public static class DeletePrepaidExpenseEndpoint
{
    public static RouteHandlerBuilder MapDeletePrepaidExpenseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeletePrepaidExpenseCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeletePrepaidExpenseEndpoint))
        .WithSummary("Delete PrepaidExpense")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.PrepaidExpenses.Delete);
    }
}
