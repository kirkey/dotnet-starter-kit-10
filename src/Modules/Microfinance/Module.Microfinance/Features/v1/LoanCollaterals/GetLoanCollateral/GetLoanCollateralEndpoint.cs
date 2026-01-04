using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.LoanCollaterals;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.LoanCollaterals.GetLoanCollateral;

public static class GetLoanCollateralEndpoint
{
    public static RouteHandlerBuilder MapGetLoanCollateralEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanCollateralQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetLoanCollateralEndpoint))
        .WithSummary("Get LoanCollateral")
        .Produces<LoanCollateralDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanCollaterals.View);
    }
}
