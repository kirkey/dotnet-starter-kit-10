using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Sources;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Sources.RefreshWebSource;

public static class RefreshWebSourceEndpoint
{
    internal static RouteHandlerBuilder MapRefreshWebSourceEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/sources/{id:guid}/refresh",
                async (Guid id, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new RefreshWebSourceCommand(id), ct).ConfigureAwait(false)))
            .WithName("RefreshWebSource")
            .WithSummary("Re-fetch a web-link source")
            .RequirePermission(AiPermissions.Sources.Create);
}
