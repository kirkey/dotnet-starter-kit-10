namespace FSH.Module.Accounting.Contracts.v1.Customers;

public record CustomerDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record CustomerSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);