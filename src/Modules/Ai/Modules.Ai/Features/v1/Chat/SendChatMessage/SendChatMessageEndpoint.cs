using System.Text.Json;
using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Shared.Identity.Authorization;
using FSH.Framework.Web.Idempotency;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Chat;
using FSH.Modules.Ai.Services;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Chat.SendChatMessage;

public sealed record SendChatMessageRequest(string Content);

public static class SendChatMessageEndpoint
{
    internal static RouteHandlerBuilder MapSendChatMessageEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/sessions/{id:guid}/messages",
                async (Guid id, SendChatMessageRequest body, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new SendChatMessageCommand(id, body.Content), ct).ConfigureAwait(false)))
            .WithName("SendChatMessage")
            .WithSummary("Send a chat message, receive the grounded answer")
            .RequirePermission(AiPermissions.Chat.Create)
            .WithIdempotency();
}

public static class SendChatMessageStreamEndpoint
{
    internal static RouteHandlerBuilder MapSendChatMessageStreamEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/sessions/{id:guid}/messages/stream",
                async Task (Guid id, SendChatMessageRequest body, IChatRunner runner, ICurrentUser user, HttpContext http, CancellationToken ct) =>
                {
                    var ownerId = user.GetUserId();
                    if (ownerId == Guid.Empty)
                    {
                        throw new UnauthorizedException("no current user");
                    }

                    if (string.IsNullOrWhiteSpace(body.Content) || body.Content.Length > 8000)
                    {
                        throw new FSH.Framework.Core.Exceptions.CustomException(
                            "Content must be between 1 and 8000 characters.",
                            errors: null,
                            System.Net.HttpStatusCode.BadRequest);
                    }

                    http.Response.Headers.ContentType = "text/event-stream";
                    http.Response.Headers.CacheControl = "no-cache";
                    http.Response.Headers.Append("X-Accel-Buffering", "no");

                    try
                    {
                        var result = await runner.RunAsync(
                            id,
                            ownerId,
                            body.Content,
                            segment => WriteEventAsync(http, new { type = "token", text = segment }, ct),
                            ct).ConfigureAwait(false);

                        await WriteEventAsync(
                            http,
                            new { type = "done", citedSources = result.AssistantMessage.CitedSources },
                            ct).ConfigureAwait(false);
                    }
                    catch (Exception ex) when (ex is not OperationCanceledException)
                    {
                        await WriteEventAsync(http, new { type = "error", message = ex.Message }, ct)
                            .ConfigureAwait(false);
                    }
                })
            .WithName("SendChatMessageStream")
            .WithSummary("Send a chat message, stream the grounded answer as SSE")
            .RequirePermission(AiPermissions.Chat.Create);

    private static async Task WriteEventAsync(HttpContext http, object payload, CancellationToken ct)
    {
        await http.Response.WriteAsync($"data: {JsonSerializer.Serialize(payload)}\n\n", ct).ConfigureAwait(false);
        await http.Response.Body.FlushAsync(ct).ConfigureAwait(false);
    }
}
