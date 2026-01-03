using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.MobileWallets.DeleteMobileWallet;

public static class DeleteMobileWalletEndpoint
{
    public static RouteHandlerBuilder MapDeleteMobileWalletEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteMobileWalletCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteMobileWalletEndpoint))
        .WithSummary("Delete MobileWallet")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.MobileWallets.Delete);
    }
}
