using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Schedules;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Schedules.ListSchedules;

public static class ListSchedulesEndpoint
{
    internal static RouteHandlerBuilder MapListSchedulesEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet("/schedules",
                async (Guid? agentId, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new ListSchedulesQuery(agentId), ct).ConfigureAwait(false)))
            .WithName("ListSchedules")
            .WithSummary("List scheduled agent tasks")
            .RequirePermission(AiPermissions.Runs.View);
}
