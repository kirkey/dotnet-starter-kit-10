using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Microfinance.Contracts.v1.AgentBankings.UpdateAgentBanking;

namespace FSH.Module.Microfinance.Features.v1.AgentBankings.UpdateAgentBanking;

public static class UpdateAgentBankingEndpoint
{
    public static RouteHandlerBuilder MapUpdateAgentBankingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateAgentBankingCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateAgentBankingEndpoint))
        .WithSummary("Update AgentBanking")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.AgentBankings.Update);
    }
}
