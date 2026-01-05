using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.LoanOfficerAssignments.UpdateLoanOfficerAssignment;

namespace FSH.Module.Microfinance.Features.v1.LoanOfficerAssignments.UpdateLoanOfficerAssignment;

public static class UpdateLoanOfficerAssignmentEndpoint
{
    public static RouteHandlerBuilder MapUpdateLoanOfficerAssignmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateLoanOfficerAssignmentCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateLoanOfficerAssignmentEndpoint))
        .WithSummary("Update LoanOfficerAssignment")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanOfficerAssignments.Update);
    }
}
