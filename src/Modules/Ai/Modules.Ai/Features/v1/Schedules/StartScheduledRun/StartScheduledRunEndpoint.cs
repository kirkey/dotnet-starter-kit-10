using FSH.Modules.Ai.Contracts.v1.Schedules;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Schedules.StartScheduledRun;

public static class StartScheduledRunEndpoint
{
    internal static RouteHandlerBuilder MapStartScheduledRunEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/schedules/{id:guid}/webhook",
                async (Guid id, string token, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new StartScheduledRunCommand(id, token), ct).ConfigureAwait(false)))
            .WithName("StartScheduledRun")
            .WithSummary("Trigger a scheduled run via webhook token (anonymous)")
            .AllowAnonymous();
}
