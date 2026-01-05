using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.MobileTransactions.DeleteMobileTransaction;

namespace FSH.Module.Microfinance.Features.v1.MobileTransactions.DeleteMobileTransaction;

public static class DeleteMobileTransactionEndpoint
{
    public static RouteHandlerBuilder MapDeleteMobileTransactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteMobileTransactionCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteMobileTransactionEndpoint))
        .WithSummary("Delete MobileTransaction")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.MobileTransactions.Delete);
    }
}
