namespace FSH.Module.Accounting.Contracts.v1.Banks;

public record BankDto(
    Guid Id,
    string BankName,
    string? BankCode,
    string? Address,
    string? ContactName,
    string? ContactPhone,
    string? RoutingNumber,
    string? SwiftCode,
    string? CurrencyCode,
    decimal OpeningBalance,
    decimal CurrentBalance,
    bool IsDefault,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record BankSummaryDto(
    Guid Id,
    string BankName,
    string? CurrencyCode,
    decimal CurrentBalance,
    bool IsDefault,
    bool IsActive);