using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.LoanDisbursementTranches.GetLoanDisbursementTranche;
using FSH.Module.Microfinance.Contracts.v1.LoanDisbursementTranches;

namespace FSH.Module.Microfinance.Features.v1.LoanDisbursementTranches.GetLoanDisbursementTranche;

public static class GetLoanDisbursementTrancheEndpoint
{
    public static RouteHandlerBuilder MapGetLoanDisbursementTrancheEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanDisbursementTrancheQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetLoanDisbursementTrancheEndpoint))
        .WithSummary("Get LoanDisbursementTranche")
        .Produces<LoanDisbursementTrancheDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanDisbursementTranches.View);
    }
}
