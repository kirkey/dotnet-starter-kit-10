using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.MobileWallets.UpdateMobileWallet;

public static class UpdateMobileWalletEndpoint
{
    public static RouteHandlerBuilder MapUpdateMobileWalletEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateMobileWalletCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateMobileWalletEndpoint))
        .WithSummary("Update MobileWallet")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.MobileWallets.Update);
    }
}
