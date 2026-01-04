using Accounting.Application.AccountsPayableAccounts.Responses;
using Accounting.Application.AccountsPayableAccounts.Search.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountsPayableAccounts.v1;

public static class ApAccountsSearchEndpoint
{
    internal static RouteHandlerBuilder MapApAccountsSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchApAccountsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ApAccountsSearchEndpoint))
            .WithSummary("Search AP accounts")
            .Produces<PagedList<ApAccountResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}


