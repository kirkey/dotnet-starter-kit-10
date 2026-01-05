using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.ShareAccounts.GetShareAccounts;
using FSH.Module.Microfinance.Contracts.v1.ShareAccounts;

namespace FSH.Module.Microfinance.Features.v1.ShareAccounts.GetShareAccounts;

public static class GetShareAccountsEndpoint
{
    public static RouteHandlerBuilder MapGetShareAccountsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetShareAccountsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetShareAccountsEndpoint))
        .WithSummary("Get ShareAccounts")
        .Produces<ShareAccountsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.ShareAccounts.Search);
    }
}
