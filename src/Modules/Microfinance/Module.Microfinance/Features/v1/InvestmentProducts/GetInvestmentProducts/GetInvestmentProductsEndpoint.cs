using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.InvestmentProducts.GetInvestmentProducts;
using FSH.Module.Microfinance.Contracts.v1.InvestmentProducts;

namespace FSH.Module.Microfinance.Features.v1.InvestmentProducts.GetInvestmentProducts;

public static class GetInvestmentProductsEndpoint
{
    public static RouteHandlerBuilder MapGetInvestmentProductsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetInvestmentProductsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetInvestmentProductsEndpoint))
        .WithSummary("Get InvestmentProducts")
        .Produces<InvestmentProductsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.InvestmentProducts.Search);
    }
}
