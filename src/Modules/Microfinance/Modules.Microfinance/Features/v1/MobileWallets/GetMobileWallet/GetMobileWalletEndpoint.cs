using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.MobileWallets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.MobileWallets.GetMobileWallet;

public static class GetMobileWalletEndpoint
{
    public static RouteHandlerBuilder MapGetMobileWalletEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetMobileWalletQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetMobileWalletEndpoint))
        .WithSummary("Get MobileWallet")
        .Produces<MobileWalletDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.MobileWallets.View);
    }
}
