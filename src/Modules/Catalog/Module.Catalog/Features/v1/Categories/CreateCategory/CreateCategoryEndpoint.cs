using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Catalog.Contracts.v1.Categories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Catalog.Features.v1.Categories.CreateCategory;

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
        .WithDescription("Creates a new category with the provided details")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(CatalogPermissionConstants.Categories.Create);
    }
}
