using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.LoanCollaterals.GetLoanCollaterals;
using FSH.Module.Microfinance.Contracts.v1.LoanCollaterals;

namespace FSH.Module.Microfinance.Features.v1.LoanCollaterals.GetLoanCollaterals;

public static class GetLoanCollateralsEndpoint
{
    public static RouteHandlerBuilder MapGetLoanCollateralsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanCollateralsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetLoanCollateralsEndpoint))
        .WithSummary("Get LoanCollaterals")
        .Produces<LoanCollateralsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanCollaterals.Search);
    }
}
