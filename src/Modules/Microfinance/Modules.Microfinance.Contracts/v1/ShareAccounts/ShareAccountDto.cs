namespace FSH.Modules.Microfinance.Contracts.v1.ShareAccounts;

public record ShareAccountDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record ShareAccountSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
