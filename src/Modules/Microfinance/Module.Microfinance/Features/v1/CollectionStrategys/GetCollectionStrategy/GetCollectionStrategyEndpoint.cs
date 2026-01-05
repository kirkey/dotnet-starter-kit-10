using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CollectionStrategys.GetCollectionStrategy;
using FSH.Module.Microfinance.Contracts.v1.CollectionStrategys;

namespace FSH.Module.Microfinance.Features.v1.CollectionStrategys.GetCollectionStrategy;

public static class GetCollectionStrategyEndpoint
{
    public static RouteHandlerBuilder MapGetCollectionStrategyEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCollectionStrategyQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCollectionStrategyEndpoint))
        .WithSummary("Get CollectionStrategy")
        .Produces<CollectionStrategyDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CollectionStrategys.View);
    }
}
