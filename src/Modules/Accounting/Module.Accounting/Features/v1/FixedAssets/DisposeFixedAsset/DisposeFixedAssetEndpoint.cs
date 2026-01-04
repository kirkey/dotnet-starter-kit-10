// TODO: Implement Dispose endpoint for FixedAsset
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.FixedAssets.DisposeFixedAsset;

public static class DisposeFixedAssetEndpoint
{
    public static RouteHandlerBuilder MapDisposeFixedAssetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DisposeFixedAssetCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(DisposeFixedAssetEndpoint))
        .WithSummary("Dispose FixedAsset")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.FixedAssets.Dispose);
    }
}
