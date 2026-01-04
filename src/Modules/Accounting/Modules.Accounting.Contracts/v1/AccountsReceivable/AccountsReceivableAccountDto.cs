namespace FSH.Modules.Accounting.Contracts.v1.AccountsReceivable;

public record AccountsReceivableAccountDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record AccountsReceivableAccountSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
