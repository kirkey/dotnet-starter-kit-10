using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.PrepaidExpenses.UpdatePrepaidExpense;

namespace FSH.Module.Accounting.Features.v1.PrepaidExpenses.UpdatePrepaidExpense;

public static class UpdatePrepaidExpenseEndpoint
{
    public static RouteHandlerBuilder MapUpdatePrepaidExpenseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdatePrepaidExpenseCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdatePrepaidExpenseEndpoint))
        .WithSummary("Update PrepaidExpense")
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.PrepaidExpenses.Update);
    }
}
