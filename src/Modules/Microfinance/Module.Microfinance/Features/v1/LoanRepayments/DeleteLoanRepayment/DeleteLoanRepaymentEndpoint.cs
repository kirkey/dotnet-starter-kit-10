using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.LoanRepayments.DeleteLoanRepayment;

namespace FSH.Module.Microfinance.Features.v1.LoanRepayments.DeleteLoanRepayment;

public static class DeleteLoanRepaymentEndpoint
{
    public static RouteHandlerBuilder MapDeleteLoanRepaymentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteLoanRepaymentCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteLoanRepaymentEndpoint))
        .WithSummary("Delete LoanRepayment")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.LoanRepayments.Delete);
    }
}
