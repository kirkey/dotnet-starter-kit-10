using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Catalog.Contracts.v1.Brands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
            var result = await mediator.Send(query, cancellationToken);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetBrandsEndpoint))
        .WithSummary("Get paginated list of brands")
        .Produces<BrandsPagedResponse>()
        .RequirePermission(CatalogPermissionConstants.Brands.View);
    }
}
