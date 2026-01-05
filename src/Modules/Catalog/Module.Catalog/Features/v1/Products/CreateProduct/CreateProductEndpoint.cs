using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Catalog.Contracts.v1.Products;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Catalog.Features.v1.Products.CreateProduct;

public static class CreateProductEndpoint
{
    public static RouteHandlerBuilder MapCreateProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/products", async (
            CreateProductCommand command,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var id = await mediator.Send(command, cancellationToken);
            return TypedResults.Created($"/api/v1/catalog/products/{id}", id);
        })
        .WithName(nameof(CreateProductEndpoint))
        .WithSummary("Create a new product")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(CatalogPermissionConstants.Products.Create);
    }
}
