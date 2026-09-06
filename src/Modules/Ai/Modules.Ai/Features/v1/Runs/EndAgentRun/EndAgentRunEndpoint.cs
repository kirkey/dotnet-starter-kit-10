using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Runs;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Runs.EndAgentRun;

public static class EndAgentRunEndpoint
{
    internal static RouteHandlerBuilder MapEndAgentRunEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/runs/{id:guid}/cancel",
                async (Guid id, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new EndAgentRunCommand(id), ct).ConfigureAwait(false)))
            .WithName("EndAgentRun")
            .WithSummary("Cancel a queued or running agent run")
            .RequirePermission(AiPermissions.Runs.Cancel);
}
