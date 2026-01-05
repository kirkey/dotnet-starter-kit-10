using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CollectionStrategys.UpdateCollectionStrategy;

namespace FSH.Module.Microfinance.Features.v1.CollectionStrategys.UpdateCollectionStrategy;

public static class UpdateCollectionStrategyEndpoint
{
    public static RouteHandlerBuilder MapUpdateCollectionStrategyEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateCollectionStrategyCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateCollectionStrategyEndpoint))
        .WithSummary("Update CollectionStrategy")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CollectionStrategys.Update);
    }
}
