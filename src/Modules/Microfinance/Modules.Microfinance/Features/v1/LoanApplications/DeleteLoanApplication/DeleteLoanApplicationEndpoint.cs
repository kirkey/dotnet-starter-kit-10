using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.LoanApplications.DeleteLoanApplication;

public static class DeleteLoanApplicationEndpoint
{
    public static RouteHandlerBuilder MapDeleteLoanApplicationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteLoanApplicationCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteLoanApplicationEndpoint))
        .WithSummary("Delete LoanApplication")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.LoanApplications.Delete);
    }
}
