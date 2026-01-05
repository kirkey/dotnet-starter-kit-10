using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.MobileWallets.GetMobileWallets;
using FSH.Module.Microfinance.Contracts.v1.MobileWallets;

namespace FSH.Module.Microfinance.Features.v1.MobileWallets.GetMobileWallets;

public static class GetMobileWalletsEndpoint
{
    public static RouteHandlerBuilder MapGetMobileWalletsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetMobileWalletsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetMobileWalletsEndpoint))
        .WithSummary("Get MobileWallets")
        .Produces<MobileWalletsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.MobileWallets.Search);
    }
}
