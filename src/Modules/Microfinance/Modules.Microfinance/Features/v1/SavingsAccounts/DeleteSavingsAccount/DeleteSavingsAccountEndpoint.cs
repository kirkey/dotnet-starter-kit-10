using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.SavingsAccounts.DeleteSavingsAccount;

public static class DeleteSavingsAccountEndpoint
{
    public static RouteHandlerBuilder MapDeleteSavingsAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteSavingsAccountCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteSavingsAccountEndpoint))
        .WithSummary("Delete SavingsAccount")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.SavingsAccounts.Delete);
    }
}
