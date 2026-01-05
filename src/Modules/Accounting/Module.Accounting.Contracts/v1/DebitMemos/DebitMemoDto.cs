namespace FSH.Module.Accounting.Contracts.v1.DebitMemos;

public record DebitMemoDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record DebitMemoSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);