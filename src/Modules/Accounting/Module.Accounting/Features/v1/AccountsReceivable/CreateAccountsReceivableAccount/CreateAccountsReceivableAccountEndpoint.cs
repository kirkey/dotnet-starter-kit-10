using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.AccountsReceivable.CreateAccountsReceivableAccount;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.AccountsReceivable.CreateAccountsReceivableAccount;

public static class CreateAccountsReceivableAccountEndpoint
{
    public static RouteHandlerBuilder MapCreateAccountsReceivableAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateAccountsReceivableAccountCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting//{id}", id);
        })
        .WithName(nameof(CreateAccountsReceivableAccountEndpoint))
        .WithSummary("Create AccountsReceivableAccount")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.AccountsReceivable.Create);
    }
}
