// TODO: Implement Amortize endpoint for PrepaidExpense
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.PrepaidExpenses.AmortizePrepaidExpense;

namespace FSH.Module.Accounting.Features.v1.PrepaidExpenses.AmortizePrepaidExpense;

public static class AmortizePrepaidExpenseEndpoint
{
    public static RouteHandlerBuilder MapAmortizePrepaidExpenseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new AmortizePrepaidExpenseCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(AmortizePrepaidExpenseEndpoint))
        .WithSummary("Amortize PrepaidExpense")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.PrepaidExpenses.Amortize);
    }
}
