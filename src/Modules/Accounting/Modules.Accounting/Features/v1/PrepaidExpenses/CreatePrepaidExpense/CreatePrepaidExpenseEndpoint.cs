using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.PrepaidExpenses.CreatePrepaidExpense;

public static class CreatePrepaidExpenseEndpoint
{
    public static RouteHandlerBuilder MapCreatePrepaidExpenseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreatePrepaidExpenseCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting//{id}", id);
        })
        .WithName(nameof(CreatePrepaidExpenseEndpoint))
        .WithSummary("Create PrepaidExpense")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.PrepaidExpenses.Create);
    }
}
