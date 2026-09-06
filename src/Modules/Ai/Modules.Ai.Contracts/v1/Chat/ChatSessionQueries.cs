using FSH.Modules.Ai.Contracts.Dtos;
using Mediator;

namespace FSH.Modules.Ai.Contracts.v1.Chat;

public sealed record ListChatSessionsQuery : IQuery<IReadOnlyList<AiChatSessionDto>>;

public sealed record GetChatSessionQuery(Guid SessionId) : IQuery<AiChatSessionDetailDto>;
