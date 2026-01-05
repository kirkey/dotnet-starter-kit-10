using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.RiskIndicators.GetRiskIndicators;
using FSH.Module.Microfinance.Contracts.v1.RiskIndicators;

namespace FSH.Module.Microfinance.Features.v1.RiskIndicators.GetRiskIndicators;

public static class GetRiskIndicatorsEndpoint
{
    public static RouteHandlerBuilder MapGetRiskIndicatorsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetRiskIndicatorsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetRiskIndicatorsEndpoint))
        .WithSummary("Get RiskIndicators")
        .Produces<RiskIndicatorsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.RiskIndicators.Search);
    }
}
