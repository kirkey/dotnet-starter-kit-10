using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.LoanOfficerTargets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.LoanOfficerTargets.GetLoanOfficerTargets;

public static class GetLoanOfficerTargetsEndpoint
{
    public static RouteHandlerBuilder MapGetLoanOfficerTargetsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanOfficerTargetsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetLoanOfficerTargetsEndpoint))
        .WithSummary("Get LoanOfficerTargets")
        .Produces<LoanOfficerTargetsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanOfficerTargets.Search);
    }
}
