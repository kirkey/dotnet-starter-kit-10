using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.CollectionActions.CreateCollectionAction;

public static class CreateCollectionActionEndpoint
{
    public static RouteHandlerBuilder MapCreateCollectionActionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateCollectionActionCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateCollectionActionEndpoint))
        .WithSummary("Create CollectionAction")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.CollectionActions.Create);
    }
}
