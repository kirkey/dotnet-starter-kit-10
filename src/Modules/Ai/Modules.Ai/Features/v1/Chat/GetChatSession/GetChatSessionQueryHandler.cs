using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Chat;
using FSH.Modules.Ai.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Chat.GetChatSession;

public sealed class GetChatSessionQueryHandler(AiDbContext db, ICurrentUser currentUser)
    : IQueryHandler<GetChatSessionQuery, AiChatSessionDetailDto>
{
    public async ValueTask<AiChatSessionDetailDto> Handle(GetChatSessionQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var ownerId = currentUser.GetUserId();
        var session = await db.ChatSessions
            .AsNoTracking()
            .Include(s => s.Messages)
            .FirstOrDefaultAsync(s => s.Id == query.SessionId && s.OwnerId == ownerId, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Chat session {query.SessionId} was not found.");

        return new AiChatSessionDetailDto(
            new AiChatSessionDto(
                session.Id, session.Title, session.Model, session.Variant,
                session.AgentId, session.LastActivityUtc, session.MessageCount),
            session.Messages
                .OrderBy(m => m.Ordinal)
                .Select(m => new AiChatMessageDto(m.Id, m.Role, m.Content, m.CitedSources()))
                .ToList());
    }
}
