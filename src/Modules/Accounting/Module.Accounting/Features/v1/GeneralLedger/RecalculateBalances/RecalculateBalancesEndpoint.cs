using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.GeneralLedger;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.GeneralLedger.RecalculateBalances;

public static class RecalculateBalancesEndpoint
{
    public static RouteHandlerBuilder MapRecalculateBalancesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/recalculate", async (
            Guid? accountId,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new RecalculateBalancesCommand(accountId), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(RecalculateBalancesEndpoint))
        .WithSummary("Recalculate chart account balances")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.ChartOfAccounts.Update);
    }
}