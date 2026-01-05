using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.MarketingCampaigns.CreateMarketingCampaign;

namespace FSH.Module.Microfinance.Features.v1.MarketingCampaigns.CreateMarketingCampaign;

public static class CreateMarketingCampaignEndpoint
{
    public static RouteHandlerBuilder MapCreateMarketingCampaignEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateMarketingCampaignCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateMarketingCampaignEndpoint))
        .WithSummary("Create MarketingCampaign")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.MarketingCampaigns.Create);
    }
}
