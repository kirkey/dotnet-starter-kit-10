using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.StaffTrainings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.StaffTrainings.GetStaffTraining;

public static class GetStaffTrainingEndpoint
{
    public static RouteHandlerBuilder MapGetStaffTrainingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetStaffTrainingQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetStaffTrainingEndpoint))
        .WithSummary("Get StaffTraining")
        .Produces<StaffTrainingDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.StaffTrainings.View);
    }
}
