using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.v1.Chat;
using FSH.Modules.Ai.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Chat.DeleteChatSession;

public sealed class DeleteChatSessionCommandHandler(AiDbContext db, ICurrentUser currentUser)
    : ICommandHandler<DeleteChatSessionCommand, Guid>
{
    public async ValueTask<Guid> Handle(DeleteChatSessionCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var ownerId = currentUser.GetUserId();
        var sessionId = await db.ChatSessions
            .Where(s => s.Id == command.SessionId && s.OwnerId == ownerId)
            .Select(s => s.Id)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
        if (sessionId == Guid.Empty)
        {
            throw new NotFoundException($"Chat session {command.SessionId} was not found.");
        }

        // ExecuteDelete bypasses cascade deletes, so remove messages explicitly first.
        await db.ChatMessages
            .Where(m => m.SessionId == sessionId)
            .ExecuteDeleteAsync(cancellationToken)
            .ConfigureAwait(false);

        await db.ChatSessions
            .Where(s => s.Id == sessionId)
            .ExecuteDeleteAsync(cancellationToken)
            .ConfigureAwait(false);

        return sessionId;
    }
}
