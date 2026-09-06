using FSH.Framework.Shared.Identity.Authorization;
using FSH.Framework.Web.Idempotency;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Chat;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Chat.CreateChatSession;

public static class CreateChatSessionEndpoint
{
    internal static RouteHandlerBuilder MapCreateChatSessionEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/sessions",
                async (CreateChatSessionCommand command, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(command, ct).ConfigureAwait(false)))
            .WithName("CreateChatSession")
            .WithSummary("Open a chat session against a model target")
            .RequirePermission(AiPermissions.Chat.Create)
            .WithIdempotency();
}
