using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.PrepaidExpenses;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.PrepaidExpenses.GetPrepaidExpense;

public static class GetPrepaidExpenseEndpoint
{
    public static RouteHandlerBuilder MapGetPrepaidExpenseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPrepaidExpenseQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetPrepaidExpenseEndpoint))
        .WithSummary("Get PrepaidExpense by ID")
        .Produces<PrepaidExpenseDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.PrepaidExpenses.View);
    }
}
