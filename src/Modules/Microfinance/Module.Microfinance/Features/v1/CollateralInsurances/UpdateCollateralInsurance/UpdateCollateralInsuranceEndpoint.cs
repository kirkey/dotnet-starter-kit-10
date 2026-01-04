using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.CollateralInsurances.UpdateCollateralInsurance;

public static class UpdateCollateralInsuranceEndpoint
{
    public static RouteHandlerBuilder MapUpdateCollateralInsuranceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateCollateralInsuranceCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateCollateralInsuranceEndpoint))
        .WithSummary("Update CollateralInsurance")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CollateralInsurances.Update);
    }
}
