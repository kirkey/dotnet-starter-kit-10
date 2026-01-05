using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CollateralValuations.UpdateCollateralValuation;

namespace FSH.Module.Microfinance.Features.v1.CollateralValuations.UpdateCollateralValuation;

public static class UpdateCollateralValuationEndpoint
{
    public static RouteHandlerBuilder MapUpdateCollateralValuationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateCollateralValuationCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateCollateralValuationEndpoint))
        .WithSummary("Update CollateralValuation")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CollateralValuations.Update);
    }
}
