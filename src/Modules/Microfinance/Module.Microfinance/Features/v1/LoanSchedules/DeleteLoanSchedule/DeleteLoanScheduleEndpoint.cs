using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.LoanSchedules.DeleteLoanSchedule;

public static class DeleteLoanScheduleEndpoint
{
    public static RouteHandlerBuilder MapDeleteLoanScheduleEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteLoanScheduleCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteLoanScheduleEndpoint))
        .WithSummary("Delete LoanSchedule")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.LoanSchedules.Delete);
    }
}
