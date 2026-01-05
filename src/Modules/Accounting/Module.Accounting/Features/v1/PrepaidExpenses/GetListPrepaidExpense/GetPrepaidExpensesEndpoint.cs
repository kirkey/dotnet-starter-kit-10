using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.PrepaidExpenses;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.PrepaidExpenses.GetListPrepaidExpense;

namespace FSH.Module.Accounting.Features.v1.PrepaidExpenses.GetPrepaidExpenses;

public static class GetPrepaidExpensesEndpoint
{
    public static RouteHandlerBuilder MapGetPrepaidExpensesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetPrepaidExpensesQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetPrepaidExpensesEndpoint))
        .WithSummary("Get paginated list of PrepaidExpenses")
        .Produces<PrepaidExpensesPagedResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.PrepaidExpenses.Search);
    }
}
