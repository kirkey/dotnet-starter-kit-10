using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.LoanGuarantors.GetLoanGuarantors;
using FSH.Module.Microfinance.Contracts.v1.LoanGuarantors;

namespace FSH.Module.Microfinance.Features.v1.LoanGuarantors.GetLoanGuarantors;

public static class GetLoanGuarantorsEndpoint
{
    public static RouteHandlerBuilder MapGetLoanGuarantorsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanGuarantorsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetLoanGuarantorsEndpoint))
        .WithSummary("Get LoanGuarantors")
        .Produces<LoanGuarantorsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanGuarantors.Search);
    }
}
