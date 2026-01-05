using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Catalog.Contracts.v1.Products;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Catalog.Features.v1.Products.GetProducts;

public static class GetProductsEndpoint
{
    public static RouteHandlerBuilder MapGetProductsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/products", async (
            [AsParameters] GetProductsQuery query,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(query, cancellationToken);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetProductsEndpoint))
        .WithSummary("Get paginated list of products")
        .Produces<ProductsPagedResponse>()
        .RequirePermission(CatalogPermissionConstants.Products.View);
    }
}
