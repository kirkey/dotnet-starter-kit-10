using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CollectionCases.UpdateCollectionCase;

namespace FSH.Module.Microfinance.Features.v1.CollectionCases.UpdateCollectionCase;

public static class UpdateCollectionCaseEndpoint
{
    public static RouteHandlerBuilder MapUpdateCollectionCaseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateCollectionCaseCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateCollectionCaseEndpoint))
        .WithSummary("Update CollectionCase")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CollectionCases.Update);
    }
}
