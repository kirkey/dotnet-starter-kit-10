using FSH.Modules.Ai.Contracts.Dtos;
using Mediator;

namespace FSH.Modules.Ai.Contracts.v1.Chat;

public sealed record CreateChatSessionCommand(
    string? Title,
    string? Model,
    AiVariant? Variant,
    Guid? AgentId) : ICommand<Guid>;
