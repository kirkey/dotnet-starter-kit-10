using FSH.Module.Catalog.Contracts.v1.Brands;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Catalog.Features.v1.Brands.CreateBrand;

public static class CreateBrandEndpoint
{
    public static RouteHandlerBuilder MapCreateBrandEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/brands", async (
            CreateBrandCommand command,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var id = await mediator.Send(command, cancellationToken);
            return TypedResults.Created($"/api/v1/catalog/brands/{id}", id);
        })
        .WithName(nameof(CreateBrandEndpoint))
        .WithSummary("Create a new brand")
        .RequirePermission(CatalogPermissionConstants.Brands.Create)
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .WithOpenApi();
    }
}
