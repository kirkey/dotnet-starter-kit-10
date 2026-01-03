using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.StaffTrainings.UpdateStaffTraining;

public static class UpdateStaffTrainingEndpoint
{
    public static RouteHandlerBuilder MapUpdateStaffTrainingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateStaffTrainingCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateStaffTrainingEndpoint))
        .WithSummary("Update StaffTraining")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.StaffTrainings.Update);
    }
}
