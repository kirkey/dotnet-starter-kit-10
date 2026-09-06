using FSH.Framework.Shared.Identity.Authorization;
using FSH.Framework.Web.Idempotency;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Runs;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Runs.StartAgentRun;

public sealed record StartAgentRunRequest(string Input);

public static class StartAgentRunEndpoint
{
    internal static RouteHandlerBuilder MapStartAgentRunEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/agents/{id:guid}/runs",
                async (Guid id, StartAgentRunRequest body, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new StartAgentRunCommand(id, body.Input), ct).ConfigureAwait(false)))
            .WithName("StartAgentRun")
            .WithSummary("Trigger a manual agent run")
            .RequirePermission(AiPermissions.Runs.Create)
            .WithIdempotency();
}
