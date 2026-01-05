using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.MobileTransactions.GetMobileTransactions;
using FSH.Module.Microfinance.Contracts.v1.MobileTransactions;

namespace FSH.Module.Microfinance.Features.v1.MobileTransactions.GetMobileTransactions;

public static class GetMobileTransactionsEndpoint
{
    public static RouteHandlerBuilder MapGetMobileTransactionsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetMobileTransactionsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetMobileTransactionsEndpoint))
        .WithSummary("Get MobileTransactions")
        .Produces<MobileTransactionsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.MobileTransactions.Search);
    }
}
