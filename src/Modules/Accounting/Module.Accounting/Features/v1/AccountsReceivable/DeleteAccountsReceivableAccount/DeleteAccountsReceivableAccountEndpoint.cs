using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.AccountsReceivable.DeleteAccountsReceivableAccount;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.AccountsReceivable.DeleteAccountsReceivableAccount;

public static class DeleteAccountsReceivableAccountEndpoint
{
    public static RouteHandlerBuilder MapDeleteAccountsReceivableAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteAccountsReceivableAccountCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteAccountsReceivableAccountEndpoint))
        .WithSummary("Delete AccountsReceivableAccount")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.AccountsReceivable.Delete);
    }
}
