namespace FSH.Modules.Accounting.Contracts.v1.AccountsPayable;

public record AccountsPayableAccountDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record AccountsPayableAccountSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
