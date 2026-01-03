using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.MarketingCampaigns;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.MarketingCampaigns.GetMarketingCampaign;

public static class GetMarketingCampaignEndpoint
{
    public static RouteHandlerBuilder MapGetMarketingCampaignEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetMarketingCampaignQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetMarketingCampaignEndpoint))
        .WithSummary("Get MarketingCampaign")
        .Produces<MarketingCampaignDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.MarketingCampaigns.View);
    }
}
