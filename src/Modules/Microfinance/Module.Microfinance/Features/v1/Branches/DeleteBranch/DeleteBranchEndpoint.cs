using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.Branches.DeleteBranch;

namespace FSH.Module.Microfinance.Features.v1.Branches.DeleteBranch;

public static class DeleteBranchEndpoint
{
    public static RouteHandlerBuilder MapDeleteBranchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteBranchCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteBranchEndpoint))
        .WithSummary("Delete Branch")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.Branches.Delete);
    }
}
