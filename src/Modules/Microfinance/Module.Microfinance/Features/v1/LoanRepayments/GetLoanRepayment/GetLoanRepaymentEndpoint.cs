using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.LoanRepayments;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.LoanRepayments.GetLoanRepayment;

public static class GetLoanRepaymentEndpoint
{
    public static RouteHandlerBuilder MapGetLoanRepaymentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanRepaymentQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetLoanRepaymentEndpoint))
        .WithSummary("Get LoanRepayment")
        .Produces<LoanRepaymentDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanRepayments.View);
    }
}
