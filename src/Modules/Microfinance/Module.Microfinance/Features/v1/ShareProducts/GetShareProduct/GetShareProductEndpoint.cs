using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.ShareProducts.GetShareProduct;
using FSH.Module.Microfinance.Contracts.v1.ShareProducts;

namespace FSH.Module.Microfinance.Features.v1.ShareProducts.GetShareProduct;

public static class GetShareProductEndpoint
{
    public static RouteHandlerBuilder MapGetShareProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetShareProductQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetShareProductEndpoint))
        .WithSummary("Get ShareProduct")
        .Produces<ShareProductDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.ShareProducts.View);
    }
}
