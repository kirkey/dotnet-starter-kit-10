using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.AccountsPayable.DeleteAccountsPayableAccount;

public static class DeleteAccountsPayableAccountEndpoint
{
    public static RouteHandlerBuilder MapDeleteAccountsPayableAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteAccountsPayableAccountCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteAccountsPayableAccountEndpoint))
        .WithSummary("Delete AccountsPayableAccount")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.AccountsPayable.Delete);
    }
}
