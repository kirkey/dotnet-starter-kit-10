// TODO: Implement Reverse endpoint for WriteOff
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.WriteOffs.ReverseWriteOff;

namespace FSH.Module.Accounting.Features.v1.WriteOffs.ReverseWriteOff;

public static class ReverseWriteOffEndpoint
{
    public static RouteHandlerBuilder MapReverseWriteOffEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ReverseWriteOffCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ReverseWriteOffEndpoint))
        .WithSummary("Reverse WriteOff")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.WriteOffs.Reverse);
    }
}
