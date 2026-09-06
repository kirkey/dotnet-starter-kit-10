using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Schedules;
using FSH.Modules.Ai.Features.v1.Schedules.GetSchedule;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Schedules.GetSchedule;

public static class GetScheduleEndpoint
{
    internal static RouteHandlerBuilder MapGetScheduleEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet("/schedules/{id:guid}",
                async (Guid id, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new GetScheduleQuery(id), ct).ConfigureAwait(false)))
            .WithName("GetSchedule")
            .WithSummary("Get a schedule with its webhook token")
            .RequirePermission(AiPermissions.Runs.View);
}
