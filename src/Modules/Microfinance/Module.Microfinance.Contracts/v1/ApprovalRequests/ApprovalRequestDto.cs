namespace FSH.Module.Microfinance.Contracts.v1.ApprovalRequests;

public record ApprovalRequestDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record ApprovalRequestSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
