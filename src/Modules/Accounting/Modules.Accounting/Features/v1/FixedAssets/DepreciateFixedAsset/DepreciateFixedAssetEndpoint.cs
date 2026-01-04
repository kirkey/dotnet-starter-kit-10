// TODO: Implement Depreciate endpoint for FixedAsset
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.FixedAssets.DepreciateFixedAsset;

public static class DepreciateFixedAssetEndpoint
{
    public static RouteHandlerBuilder MapDepreciateFixedAssetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DepreciateFixedAssetCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(DepreciateFixedAssetEndpoint))
        .WithSummary("Depreciate FixedAsset")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.FixedAssets.Depreciate);
    }
}
