using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.ShareAccounts.DeleteShareAccount;

public static class DeleteShareAccountEndpoint
{
    public static RouteHandlerBuilder MapDeleteShareAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteShareAccountCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteShareAccountEndpoint))
        .WithSummary("Delete ShareAccount")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.ShareAccounts.Delete);
    }
}
