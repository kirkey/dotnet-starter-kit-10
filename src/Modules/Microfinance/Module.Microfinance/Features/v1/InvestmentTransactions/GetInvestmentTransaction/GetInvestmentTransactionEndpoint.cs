using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.InvestmentTransactions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.InvestmentTransactions.GetInvestmentTransaction;

public static class GetInvestmentTransactionEndpoint
{
    public static RouteHandlerBuilder MapGetInvestmentTransactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetInvestmentTransactionQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetInvestmentTransactionEndpoint))
        .WithSummary("Get InvestmentTransaction")
        .Produces<InvestmentTransactionDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.InvestmentTransactions.View);
    }
}
