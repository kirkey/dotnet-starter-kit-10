using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.MemberGroups.DeleteMemberGroup;

namespace FSH.Module.Microfinance.Features.v1.MemberGroups.DeleteMemberGroup;

public static class DeleteMemberGroupEndpoint
{
    public static RouteHandlerBuilder MapDeleteMemberGroupEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteMemberGroupCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteMemberGroupEndpoint))
        .WithSummary("Delete MemberGroup")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.MemberGroups.Delete);
    }
}
