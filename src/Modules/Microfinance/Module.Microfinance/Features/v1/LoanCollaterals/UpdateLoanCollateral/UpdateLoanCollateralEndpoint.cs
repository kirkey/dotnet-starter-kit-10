using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.LoanCollaterals.UpdateLoanCollateral;

namespace FSH.Module.Microfinance.Features.v1.LoanCollaterals.UpdateLoanCollateral;

public static class UpdateLoanCollateralEndpoint
{
    public static RouteHandlerBuilder MapUpdateLoanCollateralEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateLoanCollateralCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateLoanCollateralEndpoint))
        .WithSummary("Update LoanCollateral")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanCollaterals.Update);
    }
}
