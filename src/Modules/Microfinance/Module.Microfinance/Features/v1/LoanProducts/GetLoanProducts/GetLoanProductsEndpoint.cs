using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.LoanProducts.GetLoanProducts;
using FSH.Module.Microfinance.Contracts.v1.LoanProducts;

namespace FSH.Module.Microfinance.Features.v1.LoanProducts.GetLoanProducts;

public static class GetLoanProductsEndpoint
{
    public static RouteHandlerBuilder MapGetLoanProductsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanProductsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetLoanProductsEndpoint))
        .WithSummary("Get LoanProducts")
        .Produces<LoanProductsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanProducts.Search);
    }
}
