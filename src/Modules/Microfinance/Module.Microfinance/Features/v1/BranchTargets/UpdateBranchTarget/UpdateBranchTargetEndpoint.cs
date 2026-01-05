using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.BranchTargets.UpdateBranchTarget;

namespace FSH.Module.Microfinance.Features.v1.BranchTargets.UpdateBranchTarget;

public static class UpdateBranchTargetEndpoint
{
    public static RouteHandlerBuilder MapUpdateBranchTargetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateBranchTargetCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateBranchTargetEndpoint))
        .WithSummary("Update BranchTarget")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.BranchTargets.Update);
    }
}
