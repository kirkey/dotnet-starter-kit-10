using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.CollateralValuations.CreateCollateralValuation;

public static class CreateCollateralValuationEndpoint
{
    public static RouteHandlerBuilder MapCreateCollateralValuationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateCollateralValuationCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateCollateralValuationEndpoint))
        .WithSummary("Create CollateralValuation")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.CollateralValuations.Create);
    }
}
