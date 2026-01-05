using FSH.Module.Store.Contracts.v1.POS;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Store.Features.v1.POS.GetPOSByStore;

public static class GetPOSByStoreEndpoint
{
    public static RouteHandlerBuilder MapGetPOSByStoreEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/stores/{storeId:guid}/pos", async (
            Guid storeId,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var posTerminals = await mediator.Send(new GetPOSByStoreQuery(storeId), cancellationToken);
            return TypedResults.Ok(posTerminals);
        })
        .WithName(nameof(GetPOSByStoreEndpoint))
        .WithSummary("Get POS terminals for a store")
        .RequirePermission(StorePermissionConstants.POS.View)
        .Produces<List<POSResponse>>()
        .WithOpenApi();
    }
}
