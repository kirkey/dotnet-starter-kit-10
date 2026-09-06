using FSH.Framework.Shared.Identity.Authorization;
using FSH.Framework.Web.Idempotency;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Providers;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Providers.CreateProvider;

public static class CreateProviderEndpoint
{
    internal static RouteHandlerBuilder MapCreateProviderEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/providers",
                async (CreateProviderCommand command, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(command, ct).ConfigureAwait(false)))
            .WithName("CreateProvider")
            .WithSummary("Register an AI provider")
            .RequirePermission(AiPermissions.Providers.Manage)
            .WithIdempotency();
}
