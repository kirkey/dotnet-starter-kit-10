using FSH.Framework.Shared.Identity.Authorization;
using FSH.Framework.Web.Idempotency;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Providers;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Providers.SetProviderSecret;

public sealed record SetProviderSecretRequest(string? KeyName, string Value);

public static class SetProviderSecretEndpoint
{
    internal static RouteHandlerBuilder MapSetProviderSecretEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/providers/{id:guid}/secret",
                async (Guid id, SetProviderSecretRequest body, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(
                        new SetProviderSecretCommand(id, body.KeyName, body.Value), ct).ConfigureAwait(false)))
            .WithName("SetProviderSecret")
            .WithSummary("Set or replace a provider secret (write-only)")
            .RequirePermission(AiPermissions.Providers.Manage)
            .WithIdempotency();
}
