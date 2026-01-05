using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.InvestmentTransactions.UpdateInvestmentTransaction;

namespace FSH.Module.Microfinance.Features.v1.InvestmentTransactions.UpdateInvestmentTransaction;

public static class UpdateInvestmentTransactionEndpoint
{
    public static RouteHandlerBuilder MapUpdateInvestmentTransactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateInvestmentTransactionCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateInvestmentTransactionEndpoint))
        .WithSummary("Update InvestmentTransaction")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.InvestmentTransactions.Update);
    }
}
