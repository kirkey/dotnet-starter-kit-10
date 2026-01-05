using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.ShareProducts.UpdateShareProduct;

namespace FSH.Module.Microfinance.Features.v1.ShareProducts.UpdateShareProduct;

public static class UpdateShareProductEndpoint
{
    public static RouteHandlerBuilder MapUpdateShareProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateShareProductCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateShareProductEndpoint))
        .WithSummary("Update ShareProduct")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.ShareProducts.Update);
    }
}
