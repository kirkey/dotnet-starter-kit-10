using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Providers;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Providers.UpdateProvider;

public static class UpdateProviderEndpoint
{
    internal static RouteHandlerBuilder MapUpdateProviderEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPut("/providers/{id:guid}",
                async (Guid id, UpdateProviderCommand command, IMediator mediator, CancellationToken ct) =>
                    id != command.Id
                        ? Results.BadRequest(new { error = "Route id does not match body id." })
                        : Results.Ok(await mediator.Send(command, ct).ConfigureAwait(false)))
            .WithName("UpdateProvider")
            .WithSummary("Update an AI provider (revision-guarded)")
            .RequirePermission(AiPermissions.Providers.Manage);
}
