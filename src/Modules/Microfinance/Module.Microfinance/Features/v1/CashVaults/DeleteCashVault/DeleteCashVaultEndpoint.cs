using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CashVaults.DeleteCashVault;

namespace FSH.Module.Microfinance.Features.v1.CashVaults.DeleteCashVault;

public static class DeleteCashVaultEndpoint
{
    public static RouteHandlerBuilder MapDeleteCashVaultEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteCashVaultCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteCashVaultEndpoint))
        .WithSummary("Delete CashVault")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.CashVaults.Delete);
    }
}
