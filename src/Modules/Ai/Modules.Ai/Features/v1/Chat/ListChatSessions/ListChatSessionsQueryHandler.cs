using FSH.Framework.Core.Context;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Chat;
using FSH.Modules.Ai.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Chat.ListChatSessions;

public sealed class ListChatSessionsQueryHandler(AiDbContext db, ICurrentUser currentUser)
    : IQueryHandler<ListChatSessionsQuery, IReadOnlyList<AiChatSessionDto>>
{
    public async ValueTask<IReadOnlyList<AiChatSessionDto>> Handle(ListChatSessionsQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var ownerId = currentUser.GetUserId();

        return await db.ChatSessions
            .AsNoTracking()
            .Where(s => s.OwnerId == ownerId)
            .OrderByDescending(s => s.LastActivityUtc)
            .Take(100)
            .Select(s => new AiChatSessionDto(
                s.Id, s.Title, s.Model, s.Variant, s.AgentId, s.LastActivityUtc, s.MessageCount))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
