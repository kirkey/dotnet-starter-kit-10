using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Chat;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Chat.GetChatSession;

public static class GetChatSessionEndpoint
{
    internal static RouteHandlerBuilder MapGetChatSessionEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet("/sessions/{id:guid}",
                async (Guid id, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new GetChatSessionQuery(id), ct).ConfigureAwait(false)))
            .WithName("GetChatSession")
            .WithSummary("Get a chat session with its history")
            .RequirePermission(AiPermissions.Chat.View);
}
