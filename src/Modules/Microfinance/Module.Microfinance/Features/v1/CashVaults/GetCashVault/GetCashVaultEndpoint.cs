using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CashVaults.GetCashVault;
using FSH.Module.Microfinance.Contracts.v1.CashVaults;

namespace FSH.Module.Microfinance.Features.v1.CashVaults.GetCashVault;

public static class GetCashVaultEndpoint
{
    public static RouteHandlerBuilder MapGetCashVaultEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCashVaultQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCashVaultEndpoint))
        .WithSummary("Get CashVault")
        .Produces<CashVaultDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CashVaults.View);
    }
}
