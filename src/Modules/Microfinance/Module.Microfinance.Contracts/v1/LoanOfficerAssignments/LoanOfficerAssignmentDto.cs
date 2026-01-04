namespace FSH.Module.Microfinance.Contracts.v1.LoanOfficerAssignments;

public record LoanOfficerAssignmentDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record LoanOfficerAssignmentSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
