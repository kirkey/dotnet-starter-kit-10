namespace FSH.Modules.Ai.Contracts.Dtos;

public sealed record AiSourceDto(
    Guid Id,
    string Name,
    AiSourceKind Kind,
    AiSourceStatus Status,
    string SourceRef,
    bool HasTwin,
    string? ErrorReason);
