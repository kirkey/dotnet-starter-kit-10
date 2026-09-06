using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Sources;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Sources.UpdateSourceChunks;

public static class UpdateSourceChunksEndpoint
{
    internal static RouteHandlerBuilder MapUpdateSourceChunksEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/sources/{id:guid}/rechunk",
                async (Guid id, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new UpdateSourceChunksCommand(id), ct).ConfigureAwait(false)))
            .WithName("UpdateSourceChunks")
            .WithSummary("Re-run chunking and embeddings for a source")
            .RequirePermission(AiPermissions.Sources.Create);
}
