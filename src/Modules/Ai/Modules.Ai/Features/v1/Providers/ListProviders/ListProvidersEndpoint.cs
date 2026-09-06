using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Providers;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Providers.ListProviders;

public static class ListProvidersEndpoint
{
    internal static RouteHandlerBuilder MapListProvidersEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet("/providers",
                async (IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new ListProvidersQuery(), ct).ConfigureAwait(false)))
            .WithName("ListProviders")
            .WithSummary("List AI providers (secrets redacted)")
            .RequirePermission(AiPermissions.Providers.View);
}
