using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.LoanRepayments.GetLoanRepayments;
using FSH.Module.Microfinance.Contracts.v1.LoanRepayments;

namespace FSH.Module.Microfinance.Features.v1.LoanRepayments.GetLoanRepayments;

public static class GetLoanRepaymentsEndpoint
{
    public static RouteHandlerBuilder MapGetLoanRepaymentsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanRepaymentsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetLoanRepaymentsEndpoint))
        .WithSummary("Get LoanRepayments")
        .Produces<LoanRepaymentsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanRepayments.Search);
    }
}
