using FSH.Module.Store.Contracts.v1.Stores;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Store.Features.v1.Stores.GetStores;

public static class GetStoresEndpoint
{
    public static RouteHandlerBuilder MapGetStoresEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/stores", async (
            [AsParameters] GetStoresQuery query,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var stores = await mediator.Send(query, cancellationToken);
            return TypedResults.Ok(stores);
        })
        .WithName(nameof(GetStoresEndpoint))
        .WithSummary("Get list of stores")
        .RequirePermission(StorePermissionConstants.Stores.View)
        .Produces<List<StoreResponse>>()
        .WithOpenApi();
    }
}
