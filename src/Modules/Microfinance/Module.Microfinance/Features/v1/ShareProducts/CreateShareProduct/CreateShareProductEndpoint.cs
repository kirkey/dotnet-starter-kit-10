using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.ShareProducts.CreateShareProduct;

namespace FSH.Module.Microfinance.Features.v1.ShareProducts.CreateShareProduct;

public static class CreateShareProductEndpoint
{
    public static RouteHandlerBuilder MapCreateShareProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateShareProductCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateShareProductEndpoint))
        .WithSummary("Create ShareProduct")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.ShareProducts.Create);
    }
}
