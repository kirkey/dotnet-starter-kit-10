using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.InvestmentTransactions.GetInvestmentTransactions;
using FSH.Module.Microfinance.Contracts.v1.InvestmentTransactions;

namespace FSH.Module.Microfinance.Features.v1.InvestmentTransactions.GetInvestmentTransactions;

public static class GetInvestmentTransactionsEndpoint
{
    public static RouteHandlerBuilder MapGetInvestmentTransactionsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetInvestmentTransactionsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetInvestmentTransactionsEndpoint))
        .WithSummary("Get InvestmentTransactions")
        .Produces<InvestmentTransactionsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.InvestmentTransactions.Search);
    }
}
