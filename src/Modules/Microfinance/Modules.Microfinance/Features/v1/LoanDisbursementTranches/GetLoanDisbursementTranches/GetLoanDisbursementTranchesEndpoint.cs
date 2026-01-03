using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.LoanDisbursementTranches;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.LoanDisbursementTranches.GetLoanDisbursementTranches;

public static class GetLoanDisbursementTranchesEndpoint
{
    public static RouteHandlerBuilder MapGetLoanDisbursementTranchesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanDisbursementTranchesQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetLoanDisbursementTranchesEndpoint))
        .WithSummary("Get LoanDisbursementTranches")
        .Produces<LoanDisbursementTranchesPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanDisbursementTranches.Search);
    }
}
