using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.InvestmentAccounts.GetInvestmentAccounts;
using FSH.Module.Microfinance.Contracts.v1.InvestmentAccounts;

namespace FSH.Module.Microfinance.Features.v1.InvestmentAccounts.GetInvestmentAccounts;

public static class GetInvestmentAccountsEndpoint
{
    public static RouteHandlerBuilder MapGetInvestmentAccountsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetInvestmentAccountsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetInvestmentAccountsEndpoint))
        .WithSummary("Get InvestmentAccounts")
        .Produces<InvestmentAccountsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.InvestmentAccounts.Search);
    }
}
