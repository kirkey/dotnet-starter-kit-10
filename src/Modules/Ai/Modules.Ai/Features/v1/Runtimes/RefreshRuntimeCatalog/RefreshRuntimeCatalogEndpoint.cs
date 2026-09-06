using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Runtimes;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Runtimes.RefreshRuntimeCatalog;

public static class RefreshRuntimeCatalogEndpoint
{
    internal static RouteHandlerBuilder MapRefreshRuntimeCatalogEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/runtimes/refresh",
                async (IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new RefreshRuntimeCatalogCommand(), ct).ConfigureAwait(false)))
            .WithName("RefreshRuntimeCatalog")
            .WithSummary("Refresh the runtime catalog from local detection")
            .RequirePermission(AiPermissions.Agents.Update);
}
