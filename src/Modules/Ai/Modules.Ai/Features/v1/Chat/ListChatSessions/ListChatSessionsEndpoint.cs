using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Chat;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Chat.ListChatSessions;

public static class ListChatSessionsEndpoint
{
    internal static RouteHandlerBuilder MapListChatSessionsEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet("/sessions",
                async (IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new ListChatSessionsQuery(), ct).ConfigureAwait(false)))
            .WithName("ListChatSessions")
            .WithSummary("List my chat sessions")
            .RequirePermission(AiPermissions.Chat.View);
}
