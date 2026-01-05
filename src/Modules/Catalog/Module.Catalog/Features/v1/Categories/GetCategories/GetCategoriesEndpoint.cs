using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Catalog.Contracts.v1.Categories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Catalog.Features.v1.Categories.GetCategories;

public static class GetCategoriesEndpoint
{
    public static RouteHandlerBuilder MapGetCategoriesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/categories", async (
            [AsParameters] GetCategoriesQuery query,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(query, cancellationToken);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCategoriesEndpoint))
        .WithSummary("Get paginated list of categories")
        .Produces<CategoriesPagedResponse>()
        .RequirePermission(CatalogPermissionConstants.Categories.View);
    }
}
