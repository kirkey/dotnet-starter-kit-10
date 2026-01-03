using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.LoanDisbursementTranches.DeleteLoanDisbursementTranche;

public static class DeleteLoanDisbursementTrancheEndpoint
{
    public static RouteHandlerBuilder MapDeleteLoanDisbursementTrancheEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteLoanDisbursementTrancheCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteLoanDisbursementTrancheEndpoint))
        .WithSummary("Delete LoanDisbursementTranche")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.LoanDisbursementTranches.Delete);
    }
}
