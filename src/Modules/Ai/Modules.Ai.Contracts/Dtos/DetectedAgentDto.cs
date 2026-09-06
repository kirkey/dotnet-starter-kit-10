namespace FSH.Modules.Ai.Contracts.Dtos;

public sealed record DetectedAgentDto(
    string Family,
    string DisplayName,
    string? DetectedVersion,
    bool HasCredentials);
