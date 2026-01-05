using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.AccountsPayable.UpdateAccountsPayableAccount;

namespace FSH.Module.Accounting.Features.v1.AccountsPayable.UpdateAccountsPayableAccount;

public static class UpdateAccountsPayableAccountEndpoint
{
    public static RouteHandlerBuilder MapUpdateAccountsPayableAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateAccountsPayableAccountCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateAccountsPayableAccountEndpoint))
        .WithSummary("Update AccountsPayableAccount")
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.AccountsPayable.Update);
    }
}
