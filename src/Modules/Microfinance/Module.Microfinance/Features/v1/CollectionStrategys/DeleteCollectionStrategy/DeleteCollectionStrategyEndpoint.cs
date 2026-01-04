using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.CollectionStrategys.DeleteCollectionStrategy;

public static class DeleteCollectionStrategyEndpoint
{
    public static RouteHandlerBuilder MapDeleteCollectionStrategyEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteCollectionStrategyCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteCollectionStrategyEndpoint))
        .WithSummary("Delete CollectionStrategy")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.CollectionStrategys.Delete);
    }
}
