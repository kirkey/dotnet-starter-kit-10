using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;


using FSH.Module.Microfinance.Contracts.v1.SavingsAccounts;

namespace FSH.Module.Microfinance.Features.v1.SavingsAccounts.UpdateSavingsAccount;

public static class UpdateSavingsAccountEndpoint
{
    public static RouteHandlerBuilder MapUpdateSavingsAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateSavingsAccountCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateSavingsAccountEndpoint))
        .WithSummary("Update SavingsAccount")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.SavingsAccounts.Update);
    }
}
