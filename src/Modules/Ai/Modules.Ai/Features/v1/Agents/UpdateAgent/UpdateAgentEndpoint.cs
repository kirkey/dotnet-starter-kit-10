using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Agents;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Agents.UpdateAgent;

public static class UpdateAgentEndpoint
{
    internal static RouteHandlerBuilder MapUpdateAgentEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPut("/agents/{id:guid}",
                async (Guid id, UpdateAgentCommand command, IMediator mediator, CancellationToken ct) =>
                    id != command.Id
                        ? Results.BadRequest(new { error = "Route id does not match body id." })
                        : Results.Ok(await mediator.Send(command, ct).ConfigureAwait(false)))
            .WithName("UpdateAgent")
            .WithSummary("Update a department agent")
            .RequirePermission(AiPermissions.Agents.Update);
}
