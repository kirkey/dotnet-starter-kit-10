using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.InvestmentProducts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.InvestmentProducts.GetInvestmentProduct;

public static class GetInvestmentProductEndpoint
{
    public static RouteHandlerBuilder MapGetInvestmentProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetInvestmentProductQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetInvestmentProductEndpoint))
        .WithSummary("Get InvestmentProduct")
        .Produces<InvestmentProductDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.InvestmentProducts.View);
    }
}
