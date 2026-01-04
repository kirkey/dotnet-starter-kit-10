namespace FSH.Modules.Accounting.Contracts.v1.Payees;

public record PayeeDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record PayeeSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
