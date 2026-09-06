using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Agents;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Agents.GetAgent;

public static class GetAgentEndpoint
{
    internal static RouteHandlerBuilder MapGetAgentEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet("/agents/{id:guid}",
                async (Guid id, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new GetAgentQuery(id), ct).ConfigureAwait(false)))
            .WithName("GetAgent")
            .WithSummary("Get a department agent")
            .RequirePermission(AiPermissions.Agents.View);
}
