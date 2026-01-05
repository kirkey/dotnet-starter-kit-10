using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.StaffTrainings.CreateStaffTraining;

namespace FSH.Module.Microfinance.Features.v1.StaffTrainings.CreateStaffTraining;

public static class CreateStaffTrainingEndpoint
{
    public static RouteHandlerBuilder MapCreateStaffTrainingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateStaffTrainingCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateStaffTrainingEndpoint))
        .WithSummary("Create StaffTraining")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.StaffTrainings.Create);
    }
}
