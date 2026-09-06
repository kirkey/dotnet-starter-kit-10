using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Detection;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Detection.GetLocalAgents;

public static class GetLocalAgentsEndpoint
{
    internal static RouteHandlerBuilder MapGetLocalAgentsEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet("/detection/local-agents",
                async (IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new GetLocalAgentsQuery(), ct).ConfigureAwait(false)))
            .WithName("GetLocalAgents")
            .WithSummary("List AI agents detected on this machine")
            .RequirePermission(AiPermissions.Chat.View);
}
