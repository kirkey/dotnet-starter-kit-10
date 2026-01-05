using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.LoanRestructures.GetLoanRestructures;
using FSH.Module.Microfinance.Contracts.v1.LoanRestructures;

namespace FSH.Module.Microfinance.Features.v1.LoanRestructures.GetLoanRestructures;

public static class GetLoanRestructuresEndpoint
{
    public static RouteHandlerBuilder MapGetLoanRestructuresEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanRestructuresQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetLoanRestructuresEndpoint))
        .WithSummary("Get LoanRestructures")
        .Produces<LoanRestructuresPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanRestructures.Search);
    }
}
