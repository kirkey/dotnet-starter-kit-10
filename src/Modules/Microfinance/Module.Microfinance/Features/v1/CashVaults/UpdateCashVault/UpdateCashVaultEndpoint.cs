using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CashVaults.UpdateCashVault;

namespace FSH.Module.Microfinance.Features.v1.CashVaults.UpdateCashVault;

public static class UpdateCashVaultEndpoint
{
    public static RouteHandlerBuilder MapUpdateCashVaultEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateCashVaultCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateCashVaultEndpoint))
        .WithSummary("Update CashVault")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CashVaults.Update);
    }
}
