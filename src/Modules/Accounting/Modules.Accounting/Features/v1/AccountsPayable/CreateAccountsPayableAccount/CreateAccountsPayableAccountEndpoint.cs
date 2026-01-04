using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.AccountsPayable.CreateAccountsPayableAccount;

public static class CreateAccountsPayableAccountEndpoint
{
    public static RouteHandlerBuilder MapCreateAccountsPayableAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateAccountsPayableAccountCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting//{id}", id);
        })
        .WithName(nameof(CreateAccountsPayableAccountEndpoint))
        .WithSummary("Create AccountsPayableAccount")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.AccountsPayable.Create);
    }
}
