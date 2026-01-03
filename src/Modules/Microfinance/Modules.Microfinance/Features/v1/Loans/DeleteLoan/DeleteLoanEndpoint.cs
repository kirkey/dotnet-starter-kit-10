using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.Loans.DeleteLoan;

public static class DeleteLoanEndpoint
{
    public static RouteHandlerBuilder MapDeleteLoanEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteLoanCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteLoanEndpoint))
        .WithSummary("Delete Loan")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.Loans.Delete);
    }
}
