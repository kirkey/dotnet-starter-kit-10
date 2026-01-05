// TODO: Implement Close endpoint for RetainedEarnings
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.RetainedEarnings.CloseRetainedEarnings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.RetainedEarnings.CloseRetainedEarnings;

public static class CloseRetainedEarningsEndpoint
{
    public static RouteHandlerBuilder MapCloseRetainedEarningsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            CloseRetainedEarningsCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var cmd = command with { Id = id };
            await mediator.Send(cmd, ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(CloseRetainedEarningsEndpoint))
        .WithSummary("Close RetainedEarnings")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.RetainedEarnings.Close);
    }
}
