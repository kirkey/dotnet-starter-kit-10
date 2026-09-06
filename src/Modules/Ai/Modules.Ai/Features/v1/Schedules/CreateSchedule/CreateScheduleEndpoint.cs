using FSH.Framework.Shared.Identity.Authorization;
using FSH.Framework.Web.Idempotency;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Schedules;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Schedules.CreateSchedule;

public static class CreateScheduleEndpoint
{
    internal static RouteHandlerBuilder MapCreateScheduleEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/agents/{id:guid}/schedules",
                async (Guid id, CreateScheduleCommand body, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(body with { AgentId = id }, ct).ConfigureAwait(false)))
            .WithName("CreateSchedule")
            .WithSummary("Schedule recurring agent work")
            .RequirePermission(AiPermissions.Runs.Create)
            .WithIdempotency();
}
