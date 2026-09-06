using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Runtimes;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Runtimes.ListRuntimes;

public static class ListRuntimesEndpoint
{
    internal static RouteHandlerBuilder MapListRuntimesEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet("/runtimes",
                async (IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new ListRuntimesQuery(), ct).ConfigureAwait(false)))
            .WithName("ListRuntimes")
            .WithSummary("List the runtime catalog")
            .RequirePermission(AiPermissions.Agents.View);
}
