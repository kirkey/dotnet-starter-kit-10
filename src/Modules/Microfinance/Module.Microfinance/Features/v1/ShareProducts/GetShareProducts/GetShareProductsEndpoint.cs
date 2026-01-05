using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.ShareProducts.GetShareProducts;
using FSH.Module.Microfinance.Contracts.v1.ShareProducts;

namespace FSH.Module.Microfinance.Features.v1.ShareProducts.GetShareProducts;

public static class GetShareProductsEndpoint
{
    public static RouteHandlerBuilder MapGetShareProductsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetShareProductsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetShareProductsEndpoint))
        .WithSummary("Get ShareProducts")
        .Produces<ShareProductsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.ShareProducts.Search);
    }
}
