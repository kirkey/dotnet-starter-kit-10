using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CollateralTypes.UpdateCollateralType;

namespace FSH.Module.Microfinance.Features.v1.CollateralTypes.UpdateCollateralType;

public static class UpdateCollateralTypeEndpoint
{
    public static RouteHandlerBuilder MapUpdateCollateralTypeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateCollateralTypeCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateCollateralTypeEndpoint))
        .WithSummary("Update CollateralType")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CollateralTypes.Update);
    }
}
