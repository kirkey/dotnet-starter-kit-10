using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.Loans;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.Loans.GetLoan;

public static class GetLoanEndpoint
{
    public static RouteHandlerBuilder MapGetLoanEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetLoanEndpoint))
        .WithSummary("Get Loan")
        .Produces<LoanDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.Loans.View);
    }
}
