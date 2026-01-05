using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.AccountsReceivable.UpdateAccountsReceivableAccount;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.AccountsReceivable.UpdateAccountsReceivableAccount;

public static class UpdateAccountsReceivableAccountEndpoint
{
    public static RouteHandlerBuilder MapUpdateAccountsReceivableAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateAccountsReceivableAccountCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateAccountsReceivableAccountEndpoint))
        .WithSummary("Update AccountsReceivableAccount")
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.AccountsReceivable.Update);
    }
}
