using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Providers;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Providers.DiscoverModels;

public sealed record DiscoverModelsRequest(string BaseUrl, string? ApiKey);

public static class DiscoverModelsEndpoint
{
    internal static RouteHandlerBuilder MapDiscoverModelsEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/providers/discover",
                async (DiscoverModelsRequest body, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(
                        new DiscoverModelsCommand(body.BaseUrl, body.ApiKey), ct).ConfigureAwait(false)))
            .WithName("DiscoverModels")
            .WithSummary("Discover a provider's models without persisting anything")
            .RequirePermission(AiPermissions.Providers.Manage);
}
