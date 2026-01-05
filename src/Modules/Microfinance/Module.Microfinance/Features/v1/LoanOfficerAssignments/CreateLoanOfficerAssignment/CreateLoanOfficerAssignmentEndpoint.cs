using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.LoanOfficerAssignments.CreateLoanOfficerAssignment;

namespace FSH.Module.Microfinance.Features.v1.LoanOfficerAssignments.CreateLoanOfficerAssignment;

public static class CreateLoanOfficerAssignmentEndpoint
{
    public static RouteHandlerBuilder MapCreateLoanOfficerAssignmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateLoanOfficerAssignmentCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateLoanOfficerAssignmentEndpoint))
        .WithSummary("Create LoanOfficerAssignment")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.LoanOfficerAssignments.Create);
    }
}
