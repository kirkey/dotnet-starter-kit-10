using FSH.Module.Catalog.Contracts.v1.Brands;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Catalog.Features.v1.Brands.GetBrands;

public static class GetBrandsEndpoint
{
    public static RouteHandlerBuilder MapGetBrandsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/brands", async (
            [AsParameters] GetBrandsQuery query,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var brands = await mediator.Send(query, cancellationToken);
            return TypedResults.Ok(brands);
        })
        .WithName(nameof(GetBrandsEndpoint))
        .WithSummary("Get list of brands")
        .RequirePermission(CatalogPermissionConstants.Brands.View)
        .Produces<List<BrandResponse>>()
        .WithOpenApi();
    }
}
