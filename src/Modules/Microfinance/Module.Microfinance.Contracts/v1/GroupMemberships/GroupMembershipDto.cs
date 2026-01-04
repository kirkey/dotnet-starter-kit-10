namespace FSH.Module.Microfinance.Contracts.v1.GroupMemberships;

public record GroupMembershipDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record GroupMembershipSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
