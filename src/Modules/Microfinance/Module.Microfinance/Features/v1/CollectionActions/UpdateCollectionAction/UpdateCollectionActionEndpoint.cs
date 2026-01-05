using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CollectionActions.UpdateCollectionAction;

namespace FSH.Module.Microfinance.Features.v1.CollectionActions.UpdateCollectionAction;

public static class UpdateCollectionActionEndpoint
{
    public static RouteHandlerBuilder MapUpdateCollectionActionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateCollectionActionCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateCollectionActionEndpoint))
        .WithSummary("Update CollectionAction")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CollectionActions.Update);
    }
}
