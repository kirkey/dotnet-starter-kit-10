using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Providers;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Providers.SetProviderDefault;

public sealed record SetProviderDefaultRequest(bool Chat, bool Embedding);

public static class SetProviderDefaultEndpoint
{
    internal static RouteHandlerBuilder MapSetProviderDefaultEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/providers/{id:guid}/default",
                async (Guid id, SetProviderDefaultRequest body, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(
                        new SetProviderDefaultCommand(id, body.Chat, body.Embedding), ct).ConfigureAwait(false)))
            .WithName("SetProviderDefault")
            .WithSummary("Mark a provider as the chat/embedding default")
            .RequirePermission(AiPermissions.Providers.Manage);
}
