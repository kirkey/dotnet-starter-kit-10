using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.AccountsPayable;
using FSH.Module.Accounting.Contracts.v1.AccountsPayable.GetAccountsPayableAccount;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.AccountsPayable.GetAccountsPayableAccount;

public static class GetAccountsPayableAccountEndpoint
{
    public static RouteHandlerBuilder MapGetAccountsPayableAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAccountsPayableAccountQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetAccountsPayableAccountEndpoint))
        .WithSummary("Get AccountsPayableAccount by ID")
        .Produces<AccountsPayableAccountDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.AccountsPayable.View);
    }
}
