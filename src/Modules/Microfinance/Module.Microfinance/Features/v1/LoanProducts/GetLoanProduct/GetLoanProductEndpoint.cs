using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.LoanProducts.GetLoanProduct;
using FSH.Module.Microfinance.Contracts.v1.LoanProducts;

namespace FSH.Module.Microfinance.Features.v1.LoanProducts.GetLoanProduct;

public static class GetLoanProductEndpoint
{
    public static RouteHandlerBuilder MapGetLoanProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanProductQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetLoanProductEndpoint))
        .WithSummary("Get LoanProduct")
        .Produces<LoanProductDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanProducts.View);
    }
}
