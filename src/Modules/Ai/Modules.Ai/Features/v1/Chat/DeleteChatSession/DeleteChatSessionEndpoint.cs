using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Chat;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Chat.DeleteChatSession;

public static class DeleteChatSessionEndpoint
{
    internal static RouteHandlerBuilder MapDeleteChatSessionEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapDelete("/sessions/{id:guid}",
                async (Guid id, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new DeleteChatSessionCommand(id), ct).ConfigureAwait(false)))
            .WithName("DeleteChatSession")
            .WithSummary("Delete a chat session and its history")
            .RequirePermission(AiPermissions.Chat.Delete);
}
