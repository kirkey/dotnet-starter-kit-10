using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Agents;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Agents.RestoreAgent;

public static class RestoreAgentEndpoint
{
    internal static RouteHandlerBuilder MapRestoreAgentEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/agents/{id:guid}/restore",
                async (Guid id, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new RestoreAgentCommand(id), ct).ConfigureAwait(false)))
            .WithName("RestoreAgent")
            .WithSummary("Restore an archived department agent")
            .RequirePermission(AiPermissions.Agents.Update);
}
