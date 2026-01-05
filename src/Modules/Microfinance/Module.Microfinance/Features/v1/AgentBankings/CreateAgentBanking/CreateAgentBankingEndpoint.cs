using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Microfinance.Contracts.v1.AgentBankings.CreateAgentBanking;

namespace FSH.Module.Microfinance.Features.v1.AgentBankings.CreateAgentBanking;

public static class CreateAgentBankingEndpoint
{
    public static RouteHandlerBuilder MapCreateAgentBankingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateAgentBankingCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateAgentBankingEndpoint))
        .WithSummary("Create AgentBanking")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.AgentBankings.Create);
    }
}
