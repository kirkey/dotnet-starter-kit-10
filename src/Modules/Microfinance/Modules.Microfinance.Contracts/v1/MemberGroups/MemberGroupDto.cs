namespace FSH.Modules.Microfinance.Contracts.v1.MemberGroups;

public record MemberGroupDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record MemberGroupSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
