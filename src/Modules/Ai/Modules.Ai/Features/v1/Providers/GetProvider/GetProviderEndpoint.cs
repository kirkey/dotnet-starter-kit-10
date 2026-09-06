using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Providers;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Providers.GetProvider;

public static class GetProviderEndpoint
{
    internal static RouteHandlerBuilder MapGetProviderEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet("/providers/{id:guid}",
                async (Guid id, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new GetProviderQuery(id), ct).ConfigureAwait(false)))
            .WithName("GetProvider")
            .WithSummary("Get an AI provider (secrets redacted)")
            .RequirePermission(AiPermissions.Providers.View);
}
