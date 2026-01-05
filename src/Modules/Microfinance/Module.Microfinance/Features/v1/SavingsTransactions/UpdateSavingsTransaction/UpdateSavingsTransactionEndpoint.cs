using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.SavingsTransactions.UpdateSavingsTransaction;

namespace FSH.Module.Microfinance.Features.v1.SavingsTransactions.UpdateSavingsTransaction;

public static class UpdateSavingsTransactionEndpoint
{
    public static RouteHandlerBuilder MapUpdateSavingsTransactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateSavingsTransactionCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateSavingsTransactionEndpoint))
        .WithSummary("Update SavingsTransaction")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.SavingsTransactions.Update);
    }
}
