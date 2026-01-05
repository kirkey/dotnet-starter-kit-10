using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.MarketingCampaigns.DeleteMarketingCampaign;

namespace FSH.Module.Microfinance.Features.v1.MarketingCampaigns.DeleteMarketingCampaign;

public static class DeleteMarketingCampaignEndpoint
{
    public static RouteHandlerBuilder MapDeleteMarketingCampaignEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteMarketingCampaignCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteMarketingCampaignEndpoint))
        .WithSummary("Delete MarketingCampaign")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.MarketingCampaigns.Delete);
    }
}
