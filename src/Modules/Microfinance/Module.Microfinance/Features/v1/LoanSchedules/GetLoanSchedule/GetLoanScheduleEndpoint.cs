using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.LoanSchedules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.LoanSchedules.GetLoanSchedule;

public static class GetLoanScheduleEndpoint
{
    public static RouteHandlerBuilder MapGetLoanScheduleEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanScheduleQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetLoanScheduleEndpoint))
        .WithSummary("Get LoanSchedule")
        .Produces<LoanScheduleDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanSchedules.View);
    }
}
