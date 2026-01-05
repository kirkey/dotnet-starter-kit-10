using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.SavingsProducts.DeleteSavingsProduct;

namespace FSH.Module.Microfinance.Features.v1.SavingsProducts.DeleteSavingsProduct;

public static class DeleteSavingsProductEndpoint
{
    public static RouteHandlerBuilder MapDeleteSavingsProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteSavingsProductCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteSavingsProductEndpoint))
        .WithSummary("Delete SavingsProduct")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.SavingsProducts.Delete);
    }
}
