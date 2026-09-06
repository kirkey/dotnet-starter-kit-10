using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Chat;
using FSH.Modules.Ai.Services;
using Mediator;

namespace FSH.Modules.Ai.Features.v1.Chat.SendChatMessage;

public sealed class SendChatMessageCommandHandler(
    IChatRunner runner,
    ICurrentUser currentUser)
    : ICommandHandler<SendChatMessageCommand, AiChatMessageDto>
{
    public async ValueTask<AiChatMessageDto> Handle(SendChatMessageCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var ownerId = currentUser.GetUserId();
        if (ownerId == Guid.Empty)
        {
            throw new UnauthorizedException("no current user");
        }

        var result = await runner.RunAsync(command.SessionId, ownerId, command.Content, null, cancellationToken)
            .ConfigureAwait(false);
        return result.AssistantMessage;
    }
}
