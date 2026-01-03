using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.LoanOfficerAssignments.DeleteLoanOfficerAssignment;

public static class DeleteLoanOfficerAssignmentEndpoint
{
    public static RouteHandlerBuilder MapDeleteLoanOfficerAssignmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteLoanOfficerAssignmentCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteLoanOfficerAssignmentEndpoint))
        .WithSummary("Delete LoanOfficerAssignment")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.LoanOfficerAssignments.Delete);
    }
}
