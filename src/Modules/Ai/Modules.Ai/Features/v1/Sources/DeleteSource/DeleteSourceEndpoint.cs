using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Sources;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Sources.DeleteSource;

public static class DeleteSourceEndpoint
{
    internal static RouteHandlerBuilder MapDeleteSourceEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapDelete("/sources/{id:guid}",
                async (Guid id, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new DeleteSourceCommand(id), ct).ConfigureAwait(false)))
            .WithName("DeleteSource")
            .WithSummary("Delete a source with its chunks and twin")
            .RequirePermission(AiPermissions.Sources.Delete);
}
