using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Schedules;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Schedules.DeleteSchedule;

public static class DeleteScheduleEndpoint
{
    internal static RouteHandlerBuilder MapDeleteScheduleEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapDelete("/schedules/{id:guid}",
                async (Guid id, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new DeleteScheduleCommand(id), ct).ConfigureAwait(false)))
            .WithName("DeleteSchedule")
            .WithSummary("Delete a scheduled agent task")
            .RequirePermission(AiPermissions.Runs.Create);
}
