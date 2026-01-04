using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.AccountsReceivable;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.AccountsReceivable.GetAccountsReceivableAccount;

public static class GetAccountsReceivableAccountEndpoint
{
    public static RouteHandlerBuilder MapGetAccountsReceivableAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAccountsReceivableAccountQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetAccountsReceivableAccountEndpoint))
        .WithSummary("Get AccountsReceivableAccount by ID")
        .Produces<AccountsReceivableAccountDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.AccountsReceivable.View);
    }
}
