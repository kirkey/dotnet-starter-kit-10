using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.CollectionCases.DeleteCollectionCase;

public static class DeleteCollectionCaseEndpoint
{
    public static RouteHandlerBuilder MapDeleteCollectionCaseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteCollectionCaseCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteCollectionCaseEndpoint))
        .WithSummary("Delete CollectionCase")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.CollectionCases.Delete);
    }
}
