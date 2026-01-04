using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.LoanOfficerAssignments;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.LoanOfficerAssignments.GetLoanOfficerAssignment;

public static class GetLoanOfficerAssignmentEndpoint
{
    public static RouteHandlerBuilder MapGetLoanOfficerAssignmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanOfficerAssignmentQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetLoanOfficerAssignmentEndpoint))
        .WithSummary("Get LoanOfficerAssignment")
        .Produces<LoanOfficerAssignmentDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanOfficerAssignments.View);
    }
}
