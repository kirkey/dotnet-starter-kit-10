using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.StaffTrainings.DeleteStaffTraining;

public static class DeleteStaffTrainingEndpoint
{
    public static RouteHandlerBuilder MapDeleteStaffTrainingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteStaffTrainingCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteStaffTrainingEndpoint))
        .WithSummary("Delete StaffTraining")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.StaffTrainings.Delete);
    }
}
