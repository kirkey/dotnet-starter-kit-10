using Accounting.Application.AccountsPayableAccounts.Get;
using Accounting.Application.AccountsPayableAccounts.Responses;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountsPayableAccounts.v1;

public static class ApAccountsGetEndpoint
{
    internal static RouteHandlerBuilder MapApAccountsGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetApAccountsRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ApAccountsGetEndpoint))
            .WithSummary("Get AP account by ID")
            .Produces<ApAccountResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

