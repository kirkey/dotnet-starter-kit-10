using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.SavingsProducts.CreateSavingsProduct;

namespace FSH.Module.Microfinance.Features.v1.SavingsProducts.CreateSavingsProduct;

public static class CreateSavingsProductEndpoint
{
    public static RouteHandlerBuilder MapCreateSavingsProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateSavingsProductCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateSavingsProductEndpoint))
        .WithSummary("Create SavingsProduct")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.SavingsProducts.Create);
    }
}
