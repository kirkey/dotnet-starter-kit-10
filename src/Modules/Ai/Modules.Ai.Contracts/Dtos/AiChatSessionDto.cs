namespace FSH.Modules.Ai.Contracts.Dtos;

public sealed record AiChatSessionDto(
    Guid Id,
    string Title,
    string Model,
    AiVariant Variant,
    Guid? AgentId,
    DateTimeOffset LastActivityUtc,
    int MessageCount);

public sealed record AiChatMessageDto(
    Guid Id,
    string Role,
    string Content,
    IReadOnlyList<string> CitedSources);

public sealed record AiChatSessionDetailDto(
    AiChatSessionDto Session,
    IReadOnlyList<AiChatMessageDto> Messages);
