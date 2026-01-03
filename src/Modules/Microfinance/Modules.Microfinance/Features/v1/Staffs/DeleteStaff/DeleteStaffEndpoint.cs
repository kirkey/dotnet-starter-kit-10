using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.Staffs.DeleteStaff;

public static class DeleteStaffEndpoint
{
    public static RouteHandlerBuilder MapDeleteStaffEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteStaffCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteStaffEndpoint))
        .WithSummary("Delete Staff")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.Staffs.Delete);
    }
}
