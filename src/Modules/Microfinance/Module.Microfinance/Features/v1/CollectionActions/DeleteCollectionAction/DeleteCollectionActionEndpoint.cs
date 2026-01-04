using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.CollectionActions.DeleteCollectionAction;

public static class DeleteCollectionActionEndpoint
{
    public static RouteHandlerBuilder MapDeleteCollectionActionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteCollectionActionCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteCollectionActionEndpoint))
        .WithSummary("Delete CollectionAction")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.CollectionActions.Delete);
    }
}
