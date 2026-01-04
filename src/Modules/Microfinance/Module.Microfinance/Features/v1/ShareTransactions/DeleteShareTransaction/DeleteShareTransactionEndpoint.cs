using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.ShareTransactions.DeleteShareTransaction;

public static class DeleteShareTransactionEndpoint
{
    public static RouteHandlerBuilder MapDeleteShareTransactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteShareTransactionCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteShareTransactionEndpoint))
        .WithSummary("Delete ShareTransaction")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.ShareTransactions.Delete);
    }
}
