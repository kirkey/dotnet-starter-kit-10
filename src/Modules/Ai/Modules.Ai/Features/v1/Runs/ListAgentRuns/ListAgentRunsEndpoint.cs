using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Runs;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Runs.ListAgentRuns;

public static class ListAgentRunsEndpoint
{
    internal static RouteHandlerBuilder MapListAgentRunsEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet("/runs",
                async (Guid? agentId, AgentRunStatus? status, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new ListAgentRunsQuery(agentId, status), ct).ConfigureAwait(false)))
            .WithName("ListAgentRuns")
            .WithSummary("List agent run history")
            .RequirePermission(AiPermissions.Runs.View);
}
