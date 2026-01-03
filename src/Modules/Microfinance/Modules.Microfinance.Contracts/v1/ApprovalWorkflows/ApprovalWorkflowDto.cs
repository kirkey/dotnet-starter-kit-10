namespace FSH.Modules.Microfinance.Contracts.v1.ApprovalWorkflows;

public record ApprovalWorkflowDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record ApprovalWorkflowSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
