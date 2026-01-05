using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.Checks.UpdateCheck;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Checks.UpdateCheck;

public static class UpdateCheckEndpoint
{
    public static RouteHandlerBuilder MapUpdateCheckEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateCheckCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateCheckEndpoint))
        .WithSummary("Update Check")
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Checks.Update);
    }
}