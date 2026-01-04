using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.LoanOfficerTargets.DeleteLoanOfficerTarget;

public static class DeleteLoanOfficerTargetEndpoint
{
    public static RouteHandlerBuilder MapDeleteLoanOfficerTargetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteLoanOfficerTargetCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteLoanOfficerTargetEndpoint))
        .WithSummary("Delete LoanOfficerTarget")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.LoanOfficerTargets.Delete);
    }
}
