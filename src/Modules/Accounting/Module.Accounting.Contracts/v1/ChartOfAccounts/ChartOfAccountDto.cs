namespace FSH.Module.Accounting.Contracts.v1.ChartOfAccounts;

public record ChartOfAccountDto(
    Guid Id,
    string AccountCode,
    string AccountName,
    string AccountType,
    string UsoaCategory,
    Guid? ParentAccountId,
    string ParentCode,
    decimal Balance,
    bool IsControlAccount,
    string NormalBalance,
    int AccountLevel,
    bool AllowDirectPosting,
    bool IsUsoaCompliant,
    string? RegulatoryClassification,
    string? Description,
    string? Notes,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record ChartOfAccountSummaryDto(
    Guid Id,
    string AccountCode,
    string AccountName,
    string AccountType,
    decimal Balance,
    bool IsActive);