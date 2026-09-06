using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.v1.Chat;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Domain;
using FSH.Modules.Ai.Services;
using Mediator;

namespace FSH.Modules.Ai.Features.v1.Chat.CreateChatSession;

public sealed class CreateChatSessionCommandHandler(
    AiDbContext db,
    IAiProviderSelector selector,
    ICurrentUser currentUser)
    : ICommandHandler<CreateChatSessionCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateChatSessionCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var ownerId = currentUser.GetUserId();
        if (ownerId == Guid.Empty)
        {
            throw new UnauthorizedException("no current user");
        }

        var target = await selector
            .ResolveChatTargetAsync(command.Model, command.AgentId, command.Variant, cancellationToken)
            .ConfigureAwait(false);

        var session = AiChatSession.Create(
            string.IsNullOrWhiteSpace(command.Title) ? "New chat" : command.Title.Trim(),
            target.Model,
            target.Variant,
            command.AgentId,
            ownerId);

        db.ChatSessions.Add(session);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return session.Id;
    }
}
