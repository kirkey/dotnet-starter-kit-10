using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CollateralTypes.DeleteCollateralType;

namespace FSH.Module.Microfinance.Features.v1.CollateralTypes.DeleteCollateralType;

public static class DeleteCollateralTypeEndpoint
{
    public static RouteHandlerBuilder MapDeleteCollateralTypeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteCollateralTypeCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteCollateralTypeEndpoint))
        .WithSummary("Delete CollateralType")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.CollateralTypes.Delete);
    }
}
