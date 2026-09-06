using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Schedules;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Schedules.UpdateSchedule;

public static class UpdateScheduleEndpoint
{
    internal static RouteHandlerBuilder MapUpdateScheduleEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPut("/schedules/{id:guid}",
                async (Guid id, UpdateScheduleCommand command, IMediator mediator, CancellationToken ct) =>
                    id != command.Id
                        ? Results.BadRequest(new { error = "Route id does not match body id." })
                        : Results.Ok(await mediator.Send(command, ct).ConfigureAwait(false)))
            .WithName("UpdateSchedule")
            .WithSummary("Update a scheduled agent task")
            .RequirePermission(AiPermissions.Runs.Create);
}
