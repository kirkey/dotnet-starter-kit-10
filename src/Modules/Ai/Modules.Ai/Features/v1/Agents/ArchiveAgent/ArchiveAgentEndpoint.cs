using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Agents;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Agents.ArchiveAgent;

public static class ArchiveAgentEndpoint
{
    internal static RouteHandlerBuilder MapArchiveAgentEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/agents/{id:guid}/archive",
                async (Guid id, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new ArchiveAgentCommand(id), ct).ConfigureAwait(false)))
            .WithName("ArchiveAgent")
            .WithSummary("Archive a department agent")
            .RequirePermission(AiPermissions.Agents.Update);
}
