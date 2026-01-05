using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.InvestmentAccounts.GetInvestmentAccount;
using FSH.Module.Microfinance.Contracts.v1.InvestmentAccounts;

namespace FSH.Module.Microfinance.Features.v1.InvestmentAccounts.GetInvestmentAccount;

public static class GetInvestmentAccountEndpoint
{
    public static RouteHandlerBuilder MapGetInvestmentAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetInvestmentAccountQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetInvestmentAccountEndpoint))
        .WithSummary("Get InvestmentAccount")
        .Produces<InvestmentAccountDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.InvestmentAccounts.View);
    }
}
