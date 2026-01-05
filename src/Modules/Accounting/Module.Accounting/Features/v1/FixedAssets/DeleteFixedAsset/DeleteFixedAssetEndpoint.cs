using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.FixedAssets.DeleteFixedAsset;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.FixedAssets.DeleteFixedAsset;

public static class DeleteFixedAssetEndpoint
{
    public static RouteHandlerBuilder MapDeleteFixedAssetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteFixedAssetCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteFixedAssetEndpoint))
        .WithSummary("Delete FixedAsset")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.FixedAssets.Delete);
    }
}
