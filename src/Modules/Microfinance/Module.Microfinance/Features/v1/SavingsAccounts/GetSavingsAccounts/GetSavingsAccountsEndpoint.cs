using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.SavingsAccounts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;


namespace FSH.Module.Microfinance.Features.v1.SavingsAccounts.GetSavingsAccounts;

public static class GetSavingsAccountsEndpoint
{
    public static RouteHandlerBuilder MapGetSavingsAccountsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetSavingsAccountsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetSavingsAccountsEndpoint))
        .WithSummary("Get SavingsAccounts")
        .Produces<SavingsAccountsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.SavingsAccounts.Search);
    }
}
