using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Runs;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Runs.GetAgentRun;

public static class GetAgentRunEndpoint
{
    internal static RouteHandlerBuilder MapGetAgentRunEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet("/runs/{id:guid}",
                async (Guid id, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new GetAgentRunQuery(id), ct).ConfigureAwait(false)))
            .WithName("GetAgentRun")
            .WithSummary("Get an agent run with its result")
            .RequirePermission(AiPermissions.Runs.View);
}
