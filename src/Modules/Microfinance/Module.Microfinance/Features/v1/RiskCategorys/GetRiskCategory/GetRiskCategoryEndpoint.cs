using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.RiskCategorys;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.RiskCategorys.GetRiskCategory;

public static class GetRiskCategoryEndpoint
{
    public static RouteHandlerBuilder MapGetRiskCategoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetRiskCategoryQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetRiskCategoryEndpoint))
        .WithSummary("Get RiskCategory")
        .Produces<RiskCategoryDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.RiskCategorys.View);
    }
}
