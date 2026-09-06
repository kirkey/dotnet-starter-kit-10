using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Providers;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Providers.DeleteProvider;

public static class DeleteProviderEndpoint
{
    internal static RouteHandlerBuilder MapDeleteProviderEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapDelete("/providers/{id:guid}",
                async (Guid id, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new DeleteProviderCommand(id), ct).ConfigureAwait(false)))
            .WithName("DeleteProvider")
            .WithSummary("Delete an AI provider and its secrets")
            .RequirePermission(AiPermissions.Providers.Manage);
}
