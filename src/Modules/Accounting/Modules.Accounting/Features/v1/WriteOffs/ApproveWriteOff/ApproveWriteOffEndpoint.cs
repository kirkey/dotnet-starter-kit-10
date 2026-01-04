// TODO: Implement Approve endpoint for WriteOff
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.WriteOffs.ApproveWriteOff;

public static class ApproveWriteOffEndpoint
{
    public static RouteHandlerBuilder MapApproveWriteOffEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ApproveWriteOffCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ApproveWriteOffEndpoint))
        .WithSummary("Approve WriteOff")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.WriteOffs.Approve);
    }
}
