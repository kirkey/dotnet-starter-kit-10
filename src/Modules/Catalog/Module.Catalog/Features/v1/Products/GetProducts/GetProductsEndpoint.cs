using FSH.Module.Catalog.Contracts.v1.Products;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
            var products = await mediator.Send(query, cancellationToken);
            return TypedResults.Ok(products);
        })
        .WithName(nameof(GetProductsEndpoint))
        .WithSummary("Get list of products")
        .RequirePermission(CatalogPermissionConstants.Products.View)
        .Produces<List<ProductResponse>>()
        .WithOpenApi();
    }
}
