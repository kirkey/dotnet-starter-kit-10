using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.LoanCollaterals.DeleteLoanCollateral;

public static class DeleteLoanCollateralEndpoint
{
    public static RouteHandlerBuilder MapDeleteLoanCollateralEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteLoanCollateralCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteLoanCollateralEndpoint))
        .WithSummary("Delete LoanCollateral")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.LoanCollaterals.Delete);
    }
}
