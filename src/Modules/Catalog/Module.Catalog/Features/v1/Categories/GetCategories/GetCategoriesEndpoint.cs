using FSH.Module.Catalog.Contracts.v1.Categories;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Catalog.Features.v1.Categories.GetCategories;

/// <summary>
/// Endpoint for getting list of categories.
/// </summary>
public static class GetCategoriesEndpoint
{
    public static RouteHandlerBuilder MapGetCategoriesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/categories", async (
            [AsParameters] GetCategoriesQuery query,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var categories = await mediator.Send(query, cancellationToken);
            return TypedResults.Ok(categories);
        })
        .WithName(nameof(GetCategoriesEndpoint))
        .WithSummary("Get list of categories")
        .WithDescription("Retrieves a list of categories with optional filtering by search term, parent category, and active status.")
        .RequirePermission(CatalogPermissionConstants.Categories.View)
        .Produces<List<CategoryResponse>>()
        .WithOpenApi();
    }
}
