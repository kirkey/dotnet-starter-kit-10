using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.FixedAssets.UpdateFixedAsset;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.FixedAssets.UpdateFixedAsset;

public static class UpdateFixedAssetEndpoint
{
    public static RouteHandlerBuilder MapUpdateFixedAssetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateFixedAssetCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateFixedAssetEndpoint))
        .WithSummary("Update FixedAsset")
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.FixedAssets.Update);
    }
}
