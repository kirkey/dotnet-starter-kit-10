namespace FSH.Module.Accounting.Contracts.v1.Members;

public record MemberDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record MemberSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);