using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.CollectionActions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.CollectionActions.GetCollectionAction;

public static class GetCollectionActionEndpoint
{
    public static RouteHandlerBuilder MapGetCollectionActionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCollectionActionQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCollectionActionEndpoint))
        .WithSummary("Get CollectionAction")
        .Produces<CollectionActionDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CollectionActions.View);
    }
}
