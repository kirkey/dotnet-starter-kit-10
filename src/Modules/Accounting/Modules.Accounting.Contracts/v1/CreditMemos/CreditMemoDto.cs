namespace FSH.Modules.Accounting.Contracts.v1.CreditMemos;

public record CreditMemoDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record CreditMemoSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
