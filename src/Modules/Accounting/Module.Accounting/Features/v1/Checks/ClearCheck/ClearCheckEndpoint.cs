// TODO: Implement Clear endpoint for Check
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Checks.ClearCheck;

public static class ClearCheckEndpoint
{
    public static RouteHandlerBuilder MapClearCheckEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ClearCheckCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ClearCheckEndpoint))
        .WithSummary("Clear Check")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Checks.Clear);
    }
}
