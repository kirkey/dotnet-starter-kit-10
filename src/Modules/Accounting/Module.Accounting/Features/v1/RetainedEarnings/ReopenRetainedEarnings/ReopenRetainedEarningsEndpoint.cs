// TODO: Implement Reopen endpoint for RetainedEarnings
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.RetainedEarnings.ReopenRetainedEarnings;

public static class ReopenRetainedEarningsEndpoint
{
    public static RouteHandlerBuilder MapReopenRetainedEarningsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            ReopenRetainedEarningsCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var cmd = command with { Id = id };
            await mediator.Send(cmd, ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ReopenRetainedEarningsEndpoint))
        .WithSummary("Reopen RetainedEarnings")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.RetainedEarnings.Reopen);
    }
}
