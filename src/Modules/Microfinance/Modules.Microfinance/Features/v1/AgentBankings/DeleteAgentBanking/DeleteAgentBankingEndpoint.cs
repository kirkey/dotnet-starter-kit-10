using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.AgentBankings.DeleteAgentBanking;

public static class DeleteAgentBankingEndpoint
{
    public static RouteHandlerBuilder MapDeleteAgentBankingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteAgentBankingCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteAgentBankingEndpoint))
        .WithSummary("Delete AgentBanking")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.AgentBankings.Delete);
    }
}
