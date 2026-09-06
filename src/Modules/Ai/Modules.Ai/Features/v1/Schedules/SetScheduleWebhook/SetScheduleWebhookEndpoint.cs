using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Schedules;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Schedules.SetScheduleWebhook;

public static class SetScheduleWebhookEndpoint
{
    internal static RouteHandlerBuilder MapSetScheduleWebhookEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/schedules/{id:guid}/rotate-webhook",
                async (Guid id, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new SetScheduleWebhookCommand(id), ct).ConfigureAwait(false)))
            .WithName("SetScheduleWebhook")
            .WithSummary("Rotate a schedule's webhook token")
            .RequirePermission(AiPermissions.Runs.Create);
}
