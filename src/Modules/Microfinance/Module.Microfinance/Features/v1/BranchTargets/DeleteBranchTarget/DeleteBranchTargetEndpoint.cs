using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.BranchTargets.DeleteBranchTarget;

namespace FSH.Module.Microfinance.Features.v1.BranchTargets.DeleteBranchTarget;

public static class DeleteBranchTargetEndpoint
{
    public static RouteHandlerBuilder MapDeleteBranchTargetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteBranchTargetCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteBranchTargetEndpoint))
        .WithSummary("Delete BranchTarget")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.BranchTargets.Delete);
    }
}
