using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Agents;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Agents.ListAgents;

public static class ListAgentsEndpoint
{
    internal static RouteHandlerBuilder MapListAgentsEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet("/agents",
                async (Guid? departmentId, bool? includeArchived, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(
                        new ListAgentsQuery(departmentId, includeArchived ?? false), ct).ConfigureAwait(false)))
            .WithName("ListAgents")
            .WithSummary("List department agents")
            .RequirePermission(AiPermissions.Agents.View);
}
