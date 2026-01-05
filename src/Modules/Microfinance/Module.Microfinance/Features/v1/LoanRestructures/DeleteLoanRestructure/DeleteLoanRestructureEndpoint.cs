using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.LoanRestructures.DeleteLoanRestructure;

namespace FSH.Module.Microfinance.Features.v1.LoanRestructures.DeleteLoanRestructure;

public static class DeleteLoanRestructureEndpoint
{
    public static RouteHandlerBuilder MapDeleteLoanRestructureEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteLoanRestructureCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteLoanRestructureEndpoint))
        .WithSummary("Delete LoanRestructure")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.LoanRestructures.Delete);
    }
}
