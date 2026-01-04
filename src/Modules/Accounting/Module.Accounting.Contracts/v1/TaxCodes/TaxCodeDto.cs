namespace FSH.Module.Accounting.Contracts.v1.TaxCodes;

public record TaxCodeDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record TaxCodeSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
