using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.SavingsTransactions.DeleteSavingsTransaction;

public static class DeleteSavingsTransactionEndpoint
{
    public static RouteHandlerBuilder MapDeleteSavingsTransactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteSavingsTransactionCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteSavingsTransactionEndpoint))
        .WithSummary("Delete SavingsTransaction")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.SavingsTransactions.Delete);
    }
}
