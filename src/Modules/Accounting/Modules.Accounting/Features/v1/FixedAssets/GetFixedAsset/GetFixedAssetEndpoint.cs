using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Accounting.Contracts.v1.FixedAssets;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.FixedAssets.GetFixedAsset;

public static class GetFixedAssetEndpoint
{
    public static RouteHandlerBuilder MapGetFixedAssetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetFixedAssetQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetFixedAssetEndpoint))
        .WithSummary("Get FixedAsset by ID")
        .Produces<FixedAssetDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.FixedAssets.View);
    }
}
