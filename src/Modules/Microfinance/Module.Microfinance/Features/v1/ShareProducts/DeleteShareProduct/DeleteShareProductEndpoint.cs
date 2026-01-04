using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.ShareProducts.DeleteShareProduct;

public static class DeleteShareProductEndpoint
{
    public static RouteHandlerBuilder MapDeleteShareProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteShareProductCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteShareProductEndpoint))
        .WithSummary("Delete ShareProduct")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.ShareProducts.Delete);
    }
}
