using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.MarketingCampaigns;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.MarketingCampaigns.GetMarketingCampaigns;

public static class GetMarketingCampaignsEndpoint
{
    public static RouteHandlerBuilder MapGetMarketingCampaignsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetMarketingCampaignsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetMarketingCampaignsEndpoint))
        .WithSummary("Get MarketingCampaigns")
        .Produces<MarketingCampaignsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.MarketingCampaigns.Search);
    }
}
