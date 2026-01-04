using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.AgentBankings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.AgentBankings.GetAgentBanking;

public static class GetAgentBankingEndpoint
{
    public static RouteHandlerBuilder MapGetAgentBankingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAgentBankingQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetAgentBankingEndpoint))
        .WithSummary("Get AgentBanking")
        .Produces<AgentBankingDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.AgentBankings.View);
    }
}
