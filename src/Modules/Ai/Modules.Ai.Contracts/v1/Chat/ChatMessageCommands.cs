using FSH.Modules.Ai.Contracts.Dtos;
using Mediator;

namespace FSH.Modules.Ai.Contracts.v1.Chat;

public sealed record DeleteChatSessionCommand(Guid SessionId) : ICommand<Guid>;

public sealed record SendChatMessageCommand(Guid SessionId, string Content) : ICommand<AiChatMessageDto>;
