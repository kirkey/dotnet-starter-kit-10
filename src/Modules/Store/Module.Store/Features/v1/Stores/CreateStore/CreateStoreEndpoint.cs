using FSH.Module.Store.Contracts.v1.Stores;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Store.Features.v1.Stores.CreateStore;

public static class CreateStoreEndpoint
{
    public static RouteHandlerBuilder MapCreateStoreEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/stores", async (
            CreateStoreCommand command,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var id = await mediator.Send(command, cancellationToken);
            return TypedResults.Created($"/api/v1/store/stores/{id}", id);
        })
        .WithName(nameof(CreateStoreEndpoint))
        .WithSummary("Create a new store")
        .RequirePermission(StorePermissionConstants.Stores.Create)
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .WithOpenApi();
    }
}
