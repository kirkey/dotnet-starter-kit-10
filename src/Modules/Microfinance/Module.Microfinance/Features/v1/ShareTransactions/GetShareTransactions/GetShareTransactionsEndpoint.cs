using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.ShareTransactions.GetShareTransactions;
using FSH.Module.Microfinance.Contracts.v1.ShareTransactions;

namespace FSH.Module.Microfinance.Features.v1.ShareTransactions.GetShareTransactions;

public static class GetShareTransactionsEndpoint
{
    public static RouteHandlerBuilder MapGetShareTransactionsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetShareTransactionsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetShareTransactionsEndpoint))
        .WithSummary("Get ShareTransactions")
        .Produces<ShareTransactionsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.ShareTransactions.Search);
    }
}
