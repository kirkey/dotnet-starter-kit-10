using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.SavingsProducts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.SavingsProducts.GetSavingsProduct;

public static class GetSavingsProductEndpoint
{
    public static RouteHandlerBuilder MapGetSavingsProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetSavingsProductQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetSavingsProductEndpoint))
        .WithSummary("Get SavingsProduct")
        .Produces<SavingsProductDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.SavingsProducts.View);
    }
}
