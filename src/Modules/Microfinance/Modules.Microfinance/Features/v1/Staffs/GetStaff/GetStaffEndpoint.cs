using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.Staffs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.Staffs.GetStaff;

public static class GetStaffEndpoint
{
    public static RouteHandlerBuilder MapGetStaffEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetStaffQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetStaffEndpoint))
        .WithSummary("Get Staff")
        .Produces<StaffDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.Staffs.View);
    }
}
