using FSH.Module.Catalog.Contracts.v1.Categories;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Catalog.Features.v1.Categories.CreateCategory;

/// <summary>
/// Endpoint for creating a new category.
/// </summary>
public static class CreateCategoryEndpoint
{
    public static RouteHandlerBuilder MapCreateCategoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/categories", async (
            CreateCategoryCommand command,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var id = await mediator.Send(command, cancellationToken);
            return TypedResults.Created($"/api/v1/catalog/categories/{id}", id);
        })
        .WithName(nameof(CreateCategoryEndpoint))
        .WithSummary("Create a new category")
        .WithDescription("Creates a new product category with the provided details. Returns the ID of the created category.")
        .RequirePermission(CatalogPermissionConstants.Categories.Create)
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .WithOpenApi();
    }
}
